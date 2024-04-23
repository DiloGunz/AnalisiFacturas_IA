using ADIA.Model.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADIA.Uow.SqlServer.Configurations;

public class AnalysisResponseConfiguration : IEntityTypeConfiguration<AnalysisResponse>
{
    public void Configure(EntityTypeBuilder<AnalysisResponse> b)
    {
        b.HasOne(x => x.Invoice)
            .WithOne()
            .HasForeignKey<Invoice>(y => y.IdAnalysisResponse)
            .IsRequired();

        b.HasOne(x => x.GeneralText)
            .WithOne()
            .HasForeignKey<Model.Domain.Entities.GeneralText>(y => y.IdAnalysisResponse)
            .IsRequired();
    }
}