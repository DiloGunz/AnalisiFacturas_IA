using ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;
using MediatR;

namespace ADIA.Model.DataTransfer.Queries;

public record GetAnalysisResponseByIdQuery(long Id) : IRequest<AnalysisResponseDto?>;