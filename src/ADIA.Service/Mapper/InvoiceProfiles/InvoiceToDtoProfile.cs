using ADIA.Model.DataTransfer.Dtos.InvoiceDtos;
using ADIA.Model.Domain.Entities;
using AutoMapper;

namespace ADIA.Service.Mapper.InvoiceProfiles;

public class InvoiceToDtoProfile : Profile
{
	public InvoiceToDtoProfile()
	{
		CreateMap<Invoice, InvoiceDto>();
	}
}