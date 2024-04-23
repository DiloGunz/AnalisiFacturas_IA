using ADIA.Model.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADIA.Uow.SqlServer.Configurations;

public class ItemInvoiceConfiguration : IEntityTypeConfiguration<ItemInvoice>
{
    public void Configure(EntityTypeBuilder<ItemInvoice> b)
    {
        b.Property(x => x.Description).IsRequired();
        b.Property(x => x.Quantity).HasPrecision(12, 3).IsRequired();
        b.Property(x => x.UnitPrice).HasPrecision(12, 3).IsRequired();
        b.Property(x => x.TotalAmount).HasPrecision(12, 3).IsRequired();
    }
}