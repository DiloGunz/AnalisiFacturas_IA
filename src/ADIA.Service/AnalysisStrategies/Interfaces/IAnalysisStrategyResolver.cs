using static ADIA.Shared.Enums.EntityEnums;

namespace ADIA.Service.AnalysisStrategies.Interfaces;

public interface IAnalysisStrategyResolver
{
    IAnalysisStrategy Resolve(FileType documentType);
}