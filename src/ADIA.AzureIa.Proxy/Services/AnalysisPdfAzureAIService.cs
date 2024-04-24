using ADIA.AzureIa.Proxy.Config.Models;
using ADIA.AzureIa.Proxy.Responses;
using ADIA.OpenAi.Proxy.Responses;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;

namespace ADIA.AzureIa.Proxy.Services;

/// <summary>
/// Clase para extraer el texto de un archivo pdf
/// </summary>
public class AnalysisPdfAzureAIService : IAnalysisPdfAzureAIService
{
    private readonly AzureIaConfig _azureIaConfig;
    private readonly ILogger<AnalysisPdfAzureAIService> _logger;

    public AnalysisPdfAzureAIService(AzureIaConfig azureIaConfig, ILogger<AnalysisPdfAzureAIService> logger)
    {
        _azureIaConfig = azureIaConfig ?? throw new ArgumentNullException(nameof(azureIaConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Procesa un archivo PDF utilizando el servicio de análisis de IA de Azure, extrayendo el texto y validándolo.
    /// Esta función maneja excepciones y registra los tiempos de inicio y fin del procesamiento.
    /// </summary>
    /// <param name="request">Objeto que contiene el archivo PDF a procesar y otros datos necesarios para la solicitud.</param>
    /// <returns>Una tarea que retorna un objeto AnalysisAzureIAResponse, que incluye el resultado del análisis, los tiempos de inicio y finalización, y un indicador de éxito.</returns>
    public async Task<AnalysisAzureIAResponse> ProcessPdfAsync(AnalysisAzureIARequest request)
    {
        var response = new AnalysisAzureIAResponse { Start = DateTime.Now };
        try
        {
            var textResults = await ExtractTextFromPdfAsync(request.File);
            ValidateResults(textResults);
            response.Result = string.Join("\n", textResults);
            response.Success = true;
        }
        catch (Exception ex)
        {
            HandleException(ex, response);
        }
        response.End = DateTime.Now;
        return response;
    }

    /// <summary>
    /// Extrae texto de un archivo PDF utilizando el cliente de Computer Vision de Azure.
    /// Envía el archivo para su análisis y espera los resultados de texto.
    /// </summary>
    /// <param name="fileData">Datos binarios del archivo PDF que se van a analizar.</param>
    /// <returns>Una tarea que retorna una lista de cadenas, donde cada cadena representa una sección de texto extraído del archivo PDF.</returns>

    private async Task<List<string>> ExtractTextFromPdfAsync(byte[] fileData)
    {
        var client = CreateComputerVisionClient();
        var operationId = await SubmitReadRequestAsync(client, fileData);
        return await GetTextResultsAsync(client, operationId);
    }

    /// <summary>
    /// Crea e inicializa una instancia de ComputerVisionClient con la clave API y el endpoint configurados.
    /// </summary>
    /// <returns>Una instancia configurada de ComputerVisionClient.</returns>
    private ComputerVisionClient CreateComputerVisionClient()
    {
        return new ComputerVisionClient(new ApiKeyServiceClientCredentials(_azureIaConfig.ApiKey))
        {
            Endpoint = _azureIaConfig.EndPoint
        };
    }

    /// <summary>
    /// Envía una solicitud de lectura a la API de Computer Vision con los datos del archivo y retorna el identificador de operación.
    /// </summary>
    /// <param name="client">El cliente de Computer Vision utilizado para enviar la solicitud.</param>
    /// <param name="fileData">Datos binarios del archivo para analizar.</param>
    /// <returns>Una tarea que retorna el identificador de la operación como cadena.</returns>
    private async Task<string> SubmitReadRequestAsync(ComputerVisionClient client, byte[] fileData)
    {
        var textHeaders = await client.ReadInStreamAsync(new MemoryStream(fileData));
        return ExtractOperationId(textHeaders.OperationLocation);
    }

    /// <summary>
    /// Extrae el identificador de operación de una URL de operación proporcionada.
    /// </summary>
    /// <param name="operationLocation">La URL completa de la operación desde donde se extrae el ID.</param>
    /// <returns>El identificador de la operación como cadena.</returns>
    private string ExtractOperationId(string operationLocation)
    {
        return operationLocation.Substring(operationLocation.LastIndexOf('/') + 1);
    }

    /// <summary>
    /// Obtiene los resultados del texto procesado por la API de Computer Vision, esperando hasta que la operación esté completa.
    /// </summary>
    /// <param name="client">El cliente de Computer Vision utilizado para consultar el resultado.</param>
    /// <param name="operationId">El identificador de la operación para obtener los resultados.</param>
    /// <returns>Una tarea que retorna una lista de cadenas, cada una representando una línea de texto extraída.</returns>
    private async Task<List<string>> GetTextResultsAsync(ComputerVisionClient client, string operationId)
    {
        ReadOperationResult results;
        do
        {
            results = await client.GetReadResultAsync(Guid.Parse(operationId));
        }
        while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);

        return results.AnalyzeResult.ReadResults.SelectMany(r => r.Lines).Select(l => l.Text).ToList();
    }

    /// <summary>
    /// Valida los resultados del análisis de texto para asegurarse de que contengan datos. Lanza una excepción si los resultados están vacíos.
    /// </summary>
    /// <param name="results">Lista de cadenas que contiene los resultados del texto extraído.</param>
    private void ValidateResults(List<string> results)
    {
        if (!results.Any())
            throw new InvalidOperationException("Azure IA no pudo analizar el archivo PDF.");
    }

    /// <summary>
    /// Maneja excepciones durante el proceso de análisis, registrando el error y actualizando la respuesta.
    /// </summary>
    /// <param name="ex">La excepción capturada durante el procesamiento.</param>
    /// <param name="response">La respuesta del análisis que será actualizada con detalles del fallo.</param>
    private void HandleException(Exception ex, AnalysisAzureIAResponse response)
    {
        _logger.LogError(ex, "Error processing PDF with Azure IA.");
        response.Success = false;
        response.Result = ex.Message;
    }
}