using api.Etapa;
using api.Projeto;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositorio;
using Repositorio.Interface;
using TimeZoneConverter; 

namespace service;

public class EtapaService
{
    public readonly IEtapaRepositorio _etapaRepositorio;
    public readonly AppDbContext _context;
    public EtapaService( IEtapaRepositorio etapaRepositorio, AppDbContext context)
    {
        _context = context;
        _etapaRepositorio = etapaRepositorio;
    }

 public async Task<PercentualEtapaDTO> GetPercentEtapas(int projetoid)
{
    var etapas = await _context.Etapas
        .Where(e => e.NM_PROJETO.projetoId == projetoid)
        .ToListAsync();

    decimal somaPercentExecReal = etapas.Sum(e => e.PERCENT_EXEC_REAL ?? 0);
    decimal somaPercentExecPlan = 0;

    var datasInicio = etapas.Where(e => e.DT_INICIO_PREVISTO.HasValue).Select(e => e.DT_INICIO_PREVISTO.Value);
    var datasTermino = etapas.Where(e => e.DT_TERMINO_PREVISTO.HasValue).Select(e => e.DT_TERMINO_PREVISTO.Value);

    if (!datasInicio.Any() || !datasTermino.Any())
    {
        return new PercentualEtapaDTO
        {
            PERCENT_PLANEJADO = 0,
            PERCENT_EXECUTADO = somaPercentExecReal
        };
    }

    var inicioMin = datasInicio.Min();
    var terminoMax = datasTermino.Max();
    var dataAtual = DateTime.UtcNow;
    var diffDays = (dataAtual - inicioMin).Days;
    var diffDaysTermino = 0;
    if(terminoMax < dataAtual)
        diffDaysTermino = (terminoMax - inicioMin).Days;
    else
        diffDaysTermino = (dataAtual - inicioMin).Days;
    
    if (diffDays == 0) diffDays = 1; 

    somaPercentExecPlan = diffDaysTermino*100 / (decimal)diffDays;

    return new PercentualEtapaDTO
    {
        PERCENT_PLANEJADO = somaPercentExecPlan,
        PERCENT_EXECUTADO = somaPercentExecReal
    };
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
            continue;
        }

        if (etapas.Any(e => e.SITUACAO == "atrasado para inicio" || e.SITUACAO == "atrasado para conclusão"))
        {
            atrasados++;
        }
        else if (etapas.Any(e => e.SITUACAO == "Em andamento"))
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

    if (etapa == null)
    {
        throw new KeyNotFoundException("Etapa não encontrada.");
    }

    TimeZoneInfo brasilia = TZConvert.GetTimeZoneInfo("E. South America Standard Time");

    if (inicio.DT_INICIO_PREVISTO.HasValue)
    {
        DateTime dtInicio = DateTime.SpecifyKind(inicio.DT_INICIO_PREVISTO.Value, DateTimeKind.Unspecified);
        etapa.DT_INICIO_PREVISTO = TimeZoneInfo.ConvertTimeToUtc(dtInicio, brasilia);
    }

    if (inicio.DT_TERMINO_PREVISTO.HasValue)
    {
        DateTime dtTermino = DateTime.SpecifyKind(inicio.DT_TERMINO_PREVISTO.Value, DateTimeKind.Unspecified);
        etapa.DT_TERMINO_PREVISTO = TimeZoneInfo.ConvertTimeToUtc(dtTermino, brasilia);
    }

    await _context.SaveChangesAsync();
}


}
