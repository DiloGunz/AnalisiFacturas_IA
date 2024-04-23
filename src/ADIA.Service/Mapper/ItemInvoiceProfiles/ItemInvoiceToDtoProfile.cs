using ADIA.Model.DataTransfer.Dtos.InvoiceDtos;
using ADIA.Model.Domain.Entities;
using AutoMapper;

namespace ADIA.Service.Mapper.ItemInvoiceProfiles;

public class ItemInvoiceToDtoProfile : Profile
{
	public ItemInvoiceToDtoProfile()
	{
		CreateMap<ItemInvoice, ItemInvoiceDto>();
		//CreateMap<IEnumerable<ItemInvoice>, List<ItemInvoiceDto>>();
	}
}