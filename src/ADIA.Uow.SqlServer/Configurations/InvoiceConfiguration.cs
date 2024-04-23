using ADIA.Model.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADIA.Uow.SqlServer.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> b)
    {
        b.Property(x => x.SupplierName).HasMaxLength(500).IsRequired();
        b.Property(x => x.SupplierAddress).HasMaxLength(500).IsRequired();
        b.Property(x => x.CustomerName).HasMaxLength(500).IsRequired();
        b.Property(x => x.CustomerAddress).HasMaxLength(500).IsRequired();
        b.Property(x => x.InvoiceNumber).HasMaxLength(50).IsRequired();
        b.Property(x => x.InvoiceDate).HasMaxLength(50).IsRequired();
        b.Property(x => x.Currency).HasMaxLength(10).IsRequired();
        b.Property(x => x.TotalAmmount).HasPrecision(12,3).IsRequired();

        b.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(y => y.IdInvoice)
            .IsRequired();
    }
}