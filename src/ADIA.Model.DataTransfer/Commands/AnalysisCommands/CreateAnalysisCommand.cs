using ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;
using ADIA.Shared.Response;
using MediatR;

namespace ADIA.Model.DataTransfer.Commands.AnalysisCommands;

public record CreateAnalysisCommand : IRequest<AppResponse<AnalysisResponseDto>>
{
    public string FileName { get; set; } = null!;
    public Byte[] File { get; set; }
    public string FileExtension { get; set; } = null!;
}