using api;
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
        
        var listItems = await _context.Demandas
            .Include(d => d.CATEGORIA)   
            .Include(d => d.NM_AREA_DEMANDANTE)  
            .ToListAsync();

        if (listItems == null || !listItems.Any())
        {
            return new List<Dictionary<string, object>> {
                new Dictionary<string, object> { { "message", "Demandas não encontradas." } }
            };
        }

        var result = listItems.Select(item => new Dictionary<string, object>
        {
            { "ID", item.DemandaId },
            { "NM_DEMANDA", item.NM_DEMANDA },
            { "DT_ABERTURA", item.DT_ABERTURA },
            { "DT_CONCLUSAO", item.DT_CONCLUSAO },
            { "DT_SOLICITACAO", item.DT_SOLICITACAO },
            { "CATEGORIA", item.CATEGORIA?.Nome }, // Usa o nome da categoria (evita erro se for null)
            { "NM_AREA_DEMANDANTE", item.NM_AREA_DEMANDANTE?.NM_DEMANDANTE }, // Usa o nome do demandante (evita erro se for null)
            { "NM_PO_DEMANDANTE", item.NM_PO_DEMANDANTE },
            { "NM_PO_SUBTDCR", item.NM_PO_SUBTDCR },
            { "NR_PROCESSO_SEI", item.NR_PROCESSO_SEI },
            { "PATROCINADOR", item.PATROCINADOR },
            { "PERIODICO", item.PERIODICO },
            { "PERIODICIDADE", item.PERIODICIDADE },
            { "STATUS", item.STATUS },
            { "UNIDADE", item.UNIDADE }
        }).ToList();

        return result;
    }
    catch (Exception ex)
    {
        return new List<Dictionary<string, object>> {
            new Dictionary<string, object> { { "details", ex.Message } }
        };
    }
}

public async Task CreateDemanda(DemandaDTO demanda)
{
    if (demanda.DT_SOLICITACAO.HasValue)
        demanda.DT_SOLICITACAO = demanda.DT_SOLICITACAO.Value.ToUniversalTime();
    
    if (demanda.DT_ABERTURA.HasValue)
        demanda.DT_ABERTURA = demanda.DT_ABERTURA.Value.ToUniversalTime();
    
    if (demanda.DT_CONCLUSAO.HasValue)
        demanda.DT_CONCLUSAO = demanda.DT_CONCLUSAO.Value.ToUniversalTime();

    var categoria = await _context.Categorias
        .FirstOrDefaultAsync(c => c.Nome == demanda.CATEGORIA);

    var demandante = await _context.AreaDemandantes
        .FirstOrDefaultAsync(e => e.NM_DEMANDANTE == demanda.NM_AREA_DEMANDANTE);

    if (categoria == null)
        throw new Exception("Categoria não encontrada.");
    
    if (demandante == null)
        throw new Exception("Área demandante não encontrada.");

    Demanda demandaAdd = new Demanda{
        NM_DEMANDA = demanda.NM_DEMANDA,
        CATEGORIA = categoria,
        NM_AREA_DEMANDANTE = demandante,
        DT_ABERTURA = demanda.DT_ABERTURA,
        DT_CONCLUSAO = demanda.DT_CONCLUSAO,
        DT_SOLICITACAO = demanda.DT_SOLICITACAO,
        UNIDADE = demanda.UNIDADE,
        PERIODICIDADE = demanda.PERIODICIDADE,
        PERIODICO = demanda.PERIODICO,
        NM_PO_DEMANDANTE = demanda.NM_PO_DEMANDANTE,
        NM_PO_SUBTDCR = demanda.NM_PO_SUBTDCR,
        STATUS = demanda.STATUS,
        NR_PROCESSO_SEI = demanda.NR_PROCESSO_SEI,
        PATROCINADOR = demanda.PATROCINADOR

    };
    
    _context.Demandas.Add(demandaAdd);
    await _context.SaveChangesAsync();
}


public async Task<IResult> DeleteDemanda (int id)
{   
    var item = _context.Demandas.FirstOrDefault(e => e.DemandaId == id);

    if(item == null)  return Results.NotFound();

    _context.Demandas.Remove(item);
    await _context.SaveChangesAsync();

    return Results.Ok();
}
public async Task<IResult> EditDemanda(int id, DemandaDTO demandaAtualizada)
{
    var demandaExistente = await _context.Demandas.FirstOrDefaultAsync(e => e.DemandaId == id);
    var categoria = await _context.Categorias.FirstOrDefaultAsync(e => e.Nome == demandaAtualizada.CATEGORIA);
    var demandante = await _context.AreaDemandantes.FirstOrDefaultAsync(e => e.NM_DEMANDANTE == demandaAtualizada.NM_AREA_DEMANDANTE);
    if (demandaExistente == null)
    {
        return Results.NotFound();
    }
     // Atualiza os campos com os novos valores
    
    demandaExistente.NM_DEMANDA = demandaAtualizada.NM_DEMANDA;
    demandaExistente.DT_ABERTURA = demandaAtualizada.DT_ABERTURA;
    demandaExistente.DT_CONCLUSAO = demandaAtualizada.DT_CONCLUSAO;
    demandaExistente.DT_SOLICITACAO = demandaAtualizada.DT_SOLICITACAO;
    demandaExistente.NM_AREA_DEMANDANTE = demandante;
    demandaExistente.NM_PO_DEMANDANTE = demandaAtualizada.NM_PO_DEMANDANTE;
    demandaExistente.NM_PO_SUBTDCR = demandaAtualizada.NM_PO_SUBTDCR;
    demandaExistente.NR_PROCESSO_SEI = demandaAtualizada.NR_PROCESSO_SEI;
    demandaExistente.PATROCINADOR = demandaAtualizada.PATROCINADOR;
    demandaExistente.PERIODICO = demandaAtualizada.PERIODICO;
    demandaExistente.PERIODICIDADE = demandaAtualizada.PERIODICIDADE;
    demandaExistente.STATUS = demandaAtualizada.STATUS;
    demandaExistente.UNIDADE = demandaAtualizada.UNIDADE;
    demandaExistente.CATEGORIA = categoria;
    
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

public async Task<Demanda> GetDemandaById(int id)
{
    Demanda demanda = await _context.Demandas.
    Include(e => e.CATEGORIA)
    .Include(e => e.NM_AREA_DEMANDANTE)
    .FirstOrDefaultAsync(e => e.DemandaId == id);
    return demanda;
}

public async Task<Dictionary<string, int>> GetQuantidadeTipo()
{
    return await _context.Demandas
        .GroupBy(d => d.CATEGORIA.Nome)
        .Select(g => new { Categoria = g.Key, Quantidade = g.Count() })
        .ToDictionaryAsync(g => g.Categoria, g => g.Quantidade);
}

    
}