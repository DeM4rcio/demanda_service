
using api;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Iso_Ceiling;
using Models;

namespace Repositorio;

public class ProjetoRepositorio 
{
    public readonly AppDbContext _context;
    public ProjetoRepositorio(AppDbContext context)
    {
        _context = context;
    }

   public async Task<List<Projeto>> GetProjetoListItemsAsync()
{
        List<Projeto?> listItems = await _context.Projetos.ToListAsync();
        return listItems;
}

public void CreateProjeto (Projeto projeto)
{
    _context.Projetos.Add(projeto);
    _context.SaveChangesAsync();
}

   public async Task<Projeto> GetProjetoById(int id)
{
        Projeto? item = await _context.Projetos.FirstOrDefaultAsync(e => e.projetoId == id);
        return item;
}
public async Task CreateProjetoByTemplate(Projeto projeto)
{
    using (var transaction = await _context.Database.BeginTransactionAsync())
    {
        try
        {
            Console.WriteLine("Iniciando transação...");
            
          
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();  
            Console.WriteLine($"Projeto adicionado com sucesso. Projeto ID: {projeto.projetoId}");

          
            List<Template> templates = await _context.Templates
                .Where(c => c.NM_TEMPLATE == projeto.TEMPLATE)
                .ToListAsync();
            Console.WriteLine($"Templates encontrados: {templates.Count}");

        
            if (templates.Count == 0)
            {
                throw new Exception("Nenhum template encontrado para o projeto.");
            }

            // Garantir que o projeto está rastreado pelo EF antes de criar as etapas
            _context.Attach(projeto);
            Console.WriteLine("Projeto anexado ao contexto.");

            // Criar e adicionar as etapas
            foreach (Template template in templates)
            {
                var etapa = new Etapa
                {
                    NM_ETAPA = template.NM_ETAPA,
                    NM_PROJETO = projeto,  // Garantindo a relação correta
                    PERCENT_TOTAL_ETAPA = template.PERCENT_TOTAL
                };

                // Debug: Verificar o conteúdo de cada etapa
                Console.WriteLine($"Adicionando etapa: {etapa.NM_ETAPA} (Percentual: {etapa.PERCENT_TOTAL_ETAPA})");
                
                _context.Etapas.Add(etapa);
            }

            await _context.SaveChangesAsync(); 
            Console.WriteLine("Etapas salvas com sucesso.");

            await transaction.CommitAsync(); 
            Console.WriteLine("Transação confirmada com sucesso.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(); // Reverte em caso de erro
            Console.WriteLine($"Erro: {ex.Message}");
            throw new Exception("Erro ao salvar o projeto e as etapas: " + ex.Message, ex);
        }
    }
}

public async Task AnaliseProjeto(ProjetoAnaliseDTO analise)
{
    Projeto projeto = _context.Projetos.FirstOrDefault(c => c.projetoId == analise.NM_PROJETO);
    
    ProjetoAnalise nova_analise = new ProjetoAnalise
{
    NM_PROJETO = projeto,
    ANALISE = analise.ANALISE,
    ENTRAVE = analise.ENTRAVE,
};

    _context.Analises.Add(nova_analise);
    _context.SaveChangesAsync();
    
}

public async Task<ProjetoAnalise> GetLastAnaliseProjeto(int projetoid)
{
    ProjetoAnalise projeto = _context.Analises.Where(c => c.NM_PROJETO.projetoId == projetoid).ToList().LastOrDefault();
    return projeto ; 
}

    
}