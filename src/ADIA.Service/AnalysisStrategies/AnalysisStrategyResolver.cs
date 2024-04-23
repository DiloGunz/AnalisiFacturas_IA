using ADIA.Service.AnalysisStrategies.Interfaces;
using static ADIA.Shared.Enums.EntityEnums;

namespace ADIA.Service.AnalysisStrategies;

public class AnalysisStrategyResolver : IAnalysisStrategyResolver
{
    private readonly IDictionary<FileType, IAnalysisStrategy> _strategies;

    public AnalysisStrategyResolver(IDictionary<FileType, IAnalysisStrategy> strategies)
    {
        _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
    }

    public IAnalysisStrategy Resolve(FileType documentType)
    {
        if (!_strategies.TryGetValue(documentType, out var strategy))
        {
            throw new ArgumentException("El documento no puede ser analizado.", nameof(documentType));
        }
        return strategy;
    }
}