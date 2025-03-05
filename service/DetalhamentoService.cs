using Microsoft.Graph;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph.Models;
using DotNetEnv;

public class DetalhamentoService
{
    private readonly GraphServiceClient _graphClient;
    private readonly string _siteId;
    private readonly string _listId;

    public DetalhamentoService(IConfiguration config)
    {
        var tenantId = Environment.GetEnvironmentVariable("TenantId");
        var clientId = Environment.GetEnvironmentVariable("ClientId");
        var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        _graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });

        _siteId = Environment.GetEnvironmentVariable("SiteId");
        _listId = Environment.GetEnvironmentVariable("ListDetalhamentoId");
    }

    // 1. Obter todos os itens da lista
 public async Task<List<IDictionary<string, object>>> GetDetalhesListItemsAsync(string itemId)
{
    try
    {
        var listItems = await _graphClient
            .Sites[_siteId]
            .Lists[_listId]
            .Items
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Expand = new[] { "fields" };
            });

        if (listItems?.Value == null || !listItems.Value.Any())
            throw new Exception("Nenhum item encontrado na lista.");
        
        return listItems.Value
            .Where(item => 
                           item.Fields.AdditionalData["NM_DEMANDA"].ToString() == itemId) // Filtra os registros da demanda
                           
            .Select(item =>
            {
                IDictionary<string, object> itemData = new Dictionary<string, object>(item.Fields.AdditionalData)
                {
                    { "ID", item.Id } // Adiciona o ID do item ao dicionário
                };

                return itemData;
            })
            .ToList();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao obter detalhamentos da demanda {itemId}: {ex.Message}");
        return new List<IDictionary<string, object>>(); // Retorna uma lista vazia em caso de erro
    }
}




    // 2. Obter um único item pelo ID
    public async Task<IDictionary<string, object>?> GetDetalhesListItemByIdAsync(string itemId)
    {
        try
        {
            var item = await _graphClient
                .Sites[_siteId]
                .Lists[_listId]
                .Items[itemId]
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Expand = new[] { "fields" };
                });

            return item.Fields?.AdditionalData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter item {itemId}: {ex.Message}");
            return null;
        }
    }

    // 3. Criar um novo item na lista
    public async Task<bool> CreateDetalhamentoAsync(Dictionary<string, object> fields)
    {
        try
        {
            var newItem = new ListItem
            {
                Fields = new FieldValueSet
                {
                    AdditionalData = fields
                }
            };

            await _graphClient
                .Sites[_siteId]
                .Lists[_listId]
                .Items
                .PostAsync(newItem);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("aqio");
            Console.WriteLine($"Erro ao criar item: {ex.Message}");
            return false;
        }
    }

   

}
