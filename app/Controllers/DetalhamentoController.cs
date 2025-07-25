using api.Demanda;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositorio;

namespace Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DetalhamentoController : ControllerBase
{
    private readonly DetalhamentoRepositorio _detalhamentoRepositorio;

    public DetalhamentoController(DetalhamentoRepositorio detalhamentoRepositorio)
    {
        _detalhamentoRepositorio = detalhamentoRepositorio;
    }

    [HttpGet("{demandaId}")]
    public async Task<IActionResult> GetAllDetalhamentos(int demandaId)
    {
        var detalhamentos = await _detalhamentoRepositorio.GetAllDetalhamentos(demandaId);
        return Ok(detalhamentos);
    }

   

    [HttpPost]
    public async Task<IActionResult> CreateDetalhamento(DetalhamentoDTO detalhamentoDTO)
    {
        await _detalhamentoRepositorio.CreateDetalhamento(detalhamentoDTO);
        return Ok();
    }
}