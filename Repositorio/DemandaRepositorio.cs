using demanda_service.Migrations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Iso_Ceiling;
using Models;
using Sprache;

namespace Repositorio;

public class DemandaRepositorio 
{
    public readonly AppDbContext _context;
    public DemandaRepositorio(AppDbContext context)
    {
        _context = context;
    }

   public async Task<List<Dictionary<string, object>>> GetDemandaListItemsAsync()
{
    try
    {
        var listItems = await _context.Demandas.ToListAsync();

        if (listItems == null || !listItems.Any())
        {
            return new List<Dictionary<string, object>> {
                new Dictionary<string, object> { { "message", "Demandas não encontrados." } }
            };
        }

        var result = listItems.Select(item => new Dictionary<string, object>
        {
            { "ID", item.DemandaId },
            { "NM_DEMANDA", item.NM_DEMANDA },
            {"DT_ABERTURA", item.DT_ABERTURA},
            {"DT_CONCLUSAO", item.DT_CONCLUSAO},
            {"DT_SOLICITACAO", item.DT_SOLICITACAO},
            {"NM_AREA_DEMANDANTE", item.NM_AREA_DEMANDANTE},
            {"NM_PO_DEMANDANTE", item.NM_PO_DEMANDANTE},
            {"NM_PO_SUBTDCR", item.NM_PO_SUBTDCR},
            {"NR_PROCESSO_SEI", item.NR_PROCESSO_SEI},
            {"PATROCINADOR", item.PATROCINADOR},
            {"PERIODICO", item.PERIODICO},
            {"PERIODICIDADE", item.PERIODICIDADE},
            {"STATUS", item.STATUS},
            {"UNIDADE", item.UNIDADE}
        }).ToList();

        return result;
    }
    catch (Exception ex)
    {
        return new List<Dictionary<string, object>> {
            new Dictionary<string, object> { {  "details", ex.Message } }
        };
    }
}

public void CreateDemanda (Demanda demanda)
{
    _context.Demandas.Add(demanda);
    _context.SaveChangesAsync();
}

public async Task<IResult> DeleteDemanda (int id)
{   
    var item = _context.Demandas.FirstOrDefault(e => e.DemandaId == id);

    if(item == null)  return Results.NotFound();

    _context.Demandas.Remove(item);
    await _context.SaveChangesAsync();

    return Results.Ok();
}
public async Task<IResult> EditDemanda(int id, Demanda demandaAtualizada)
{
    if (demandaAtualizada == null)
    {
        return Results.BadRequest("Dados da demanda não fornecidos.");
    }

    var demandaExistente = await _context.Demandas.FirstOrDefaultAsync(e => e.DemandaId == id);
    if (demandaExistente == null)
    {
        return Results.NotFound();
    }
     // Atualiza os campos com os novos valores
    
    demandaExistente.NM_DEMANDA = demandaAtualizada.NM_DEMANDA;
    demandaExistente.DT_ABERTURA = demandaAtualizada.DT_ABERTURA;
    demandaExistente.DT_CONCLUSAO = demandaAtualizada.DT_CONCLUSAO;
    demandaExistente.DT_SOLICITACAO = demandaAtualizada.DT_SOLICITACAO;
    demandaExistente.NM_AREA_DEMANDANTE = demandaAtualizada.NM_AREA_DEMANDANTE;
    demandaExistente.NM_PO_DEMANDANTE = demandaAtualizada.NM_PO_DEMANDANTE;
    demandaExistente.NM_PO_SUBTDCR = demandaAtualizada.NM_PO_SUBTDCR;
    demandaExistente.NR_PROCESSO_SEI = demandaAtualizada.NR_PROCESSO_SEI;
    demandaExistente.PATROCINADOR = demandaAtualizada.PATROCINADOR;
    demandaExistente.PERIODICO = demandaAtualizada.PERIODICO;
    demandaExistente.PERIODICIDADE = demandaAtualizada.PERIODICIDADE;
    demandaExistente.STATUS = demandaAtualizada.STATUS;
    demandaExistente.UNIDADE = demandaAtualizada.UNIDADE;
    demandaExistente.CATEGORIA = demandaAtualizada.CATEGORIA;
    
    try
    {

        await _context.SaveChangesAsync(); 
        
        return Results.Ok();
    }
    catch (DbUpdateException ex)
    {
        // Retorne o erro detalhado
        return Results.StatusCode(500);
    }
}


    
}