using ADIA.Model.DataTransfer.Dtos.GeneralTextDtos;
using ADIA.Model.Domain.Entities;
using AutoMapper;

namespace ADIA.Service.Mapper.GeneralTextProfiles;

public class GeneralTextToDtoProfile : Profile
{
	public GeneralTextToDtoProfile()
	{
		CreateMap<GeneralText, GeneralTextDto>();
	}
}