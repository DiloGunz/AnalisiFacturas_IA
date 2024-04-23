using ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;
using ADIA.Model.Domain.Entities;
using AutoMapper;

namespace ADIA.Service.Mapper.AnalysisResponseProfiles;

public class AnalysisResponseToDtoProfile : Profile
{
	public AnalysisResponseToDtoProfile()
	{
		CreateMap<AnalysisResponse, AnalysisResponseDto>();
		CreateMap<AnalysisResponse, AnalysisResponseListDto>();
	}
}