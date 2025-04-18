using api;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositorio;

namespace service;

public class EtapaService
{
    public readonly EtapaRepositorio _etapaRepositorio;
    public readonly AppDbContext _context;
    public EtapaService( EtapaRepositorio etapaRepositorio, AppDbContext context)
    {
        _context = context;
        _etapaRepositorio = etapaRepositorio;
    }

    public async Task<PercentualEtapaDTO> GetPercentEtapas (int projetoid)
    {
        var etapas = await _context.Etapas
    .Where(e => e.NM_PROJETO.projetoId == projetoid)
    .ToListAsync(); 

    decimal? somaPercentExecReal = etapas.Sum(e => e.PERCENT_EXEC_REAL ?? 0);
    decimal? somaPercentExecPlan = etapas.Sum(e => e.PERCENT_PLANEJADO);

        
        PercentualEtapaDTO response = new PercentualEtapaDTO {
            PERCENT_PLANEJADO = somaPercentExecPlan,
            PERCENT_EXECUTADO = somaPercentExecReal,
        };

        return response ;
    }
    
 public async Task<SituacaoProjetoDTO> GetSituacao()
{
    var projetos = await _context.Projetos.ToListAsync();

    int concluidos = 0;
    int atrasados = 0;
    int emAndamento = 0;
    int naoIniciados = 0;

    foreach (var projeto in projetos)
    {
        var etapas = await _context.Etapas
            .Where(e => e.NM_PROJETO.projetoId == projeto.projetoId)
            .ToListAsync();

        if (etapas.Count == 0)
        {
            naoIniciados++;
            continue;
        }

        if (etapas.Any(e => e.SITUACAO == "Atrasado"))
        {
            atrasados++;
        }
        else if (etapas.Any(e => e.SITUACAO == "Em Andamento"))
        {
            emAndamento++;
        }
        else if (etapas.All(e => e.SITUACAO == "Concluido"))
        {
            concluidos++;
        }
        else
        {
            naoIniciados++;
        }
    }

    return new SituacaoProjetoDTO
    {
        Atrasado = atrasados,
        EmAndamento = emAndamento,
        Concluido = concluidos,
        NaoIniciado = naoIniciados
    };
}

public async Task<TagsDTO> GetTags()
{
    var pdtic2427 = await _context.Projetos.Where(p => p.PDTIC2427 == true).ToListAsync();
    var ptd2427 = await _context.Projetos.Where(p => p.PTD2427 == true).ToListAsync();
    var profiscoII = await _context.Projetos.Where(p => p.PROFISCOII == true).ToListAsync();

    return new TagsDTO {
        PDTIC2427 = pdtic2427.Count,
        PTD2427 = ptd2427.Count,
        PROFISCOII = profiscoII.Count
    };
}

public async Task IniciarEtapa(InicioEtapaDTO inicio)
{
    Etapa etapa = await _etapaRepositorio.GetById(inicio.EtapaProjetoId);

    etapa.DT_INICIO_PREVISTO = inicio.DT_INICIO_PREVISTO.Value.ToUniversalTime();
    etapa.DT_TERMINO_PREVISTO = inicio.DT_TERMINO_PREVISTO.Value.ToUniversalTime();

    _context.SaveChangesAsync();
}

}
