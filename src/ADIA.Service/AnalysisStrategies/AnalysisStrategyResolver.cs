using ADIA.Service.AnalysisStrategies.Interfaces;
using static ADIA.Shared.Enums.EntityEnums;

namespace ADIA.Service.AnalysisStrategies;

/// <summary>
/// Resuelve y proporciona la estrategia adecuada de análisis basada en el tipo de archivo, utilizando un diccionario de estrategias.
/// </summary>
public class AnalysisStrategyResolver : IAnalysisStrategyResolver
{
    private readonly IDictionary<FileType, IAnalysisStrategy> _strategies;

    /// <summary>
    /// Inicializa una nueva instancia de la clase AnalysisStrategyResolver con un diccionario predefinido de estrategias.
    /// </summary>
    /// <param name="strategies">Diccionario que mapea tipos de archivos a sus correspondientes estrategias de análisis.</param>
    public AnalysisStrategyResolver(IDictionary<FileType, IAnalysisStrategy> strategies)
    {
        _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
    }

    /// <summary>
    /// Resuelve y retorna la estrategia de análisis apropiada para el tipo de archivo dado.
    /// </summary>
    /// <param name="documentType">El tipo de archivo para el cual se necesita una estrategia de análisis.</param>
    /// <returns>La estrategia de análisis correspondiente al tipo de archivo proporcionado.</returns>
    public IAnalysisStrategy Resolve(FileType documentType)
    {
        if (!_strategies.TryGetValue(documentType, out var strategy))
        {
            throw new ArgumentException("El documento no puede ser analizado.", nameof(documentType));
        }
        return strategy;
    }
}