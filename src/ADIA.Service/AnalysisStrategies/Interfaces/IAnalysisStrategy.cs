using ADIA.Model.DataTransfer.Commands.AnalysisCommands;
using ADIA.Model.DataTransfer.IaResponses;

namespace ADIA.Service.AnalysisStrategies.Interfaces;

public interface IAnalysisStrategy
{
    Task<AnalysisIAResultDto> AnalyzeAsync(CreateAnalysisCommand command);
}