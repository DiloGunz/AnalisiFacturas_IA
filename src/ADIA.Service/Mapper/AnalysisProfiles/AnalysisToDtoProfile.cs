using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Model.Domain.Entities;
using ADIA.Shared.Collection;
using AutoMapper;

namespace ADIA.Service.Mapper.AnalysisProfiles;

public class AnalysisToDtoProfile : Profile
{
	public AnalysisToDtoProfile()
	{
        CreateMap<Analysis, AnalysisListDto>()
			.ForMember(dest => dest.AnalysisResponseListDto, opt => opt.MapFrom(src => src.AnalysisResponse));
		CreateMap<DataCollection<Analysis>, DataCollection<AnalysisListDto>>();
	}
}