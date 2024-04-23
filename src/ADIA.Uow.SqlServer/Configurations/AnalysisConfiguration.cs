using ADIA.Model.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADIA.Uow.SqlServer.Configurations;

public class AnalysisConfiguration : IEntityTypeConfiguration<Analysis>
{
    public void Configure(EntityTypeBuilder<Analysis> b)
    {
        b.Property(x => x.FileName).HasMaxLength(500).IsRequired();
        b.Property(x => x.FileBase64).IsRequired();
        b.Property(x => x.FileExtension).HasMaxLength(4).IsRequired();

        b.HasOne(x => x.AnalysisResponse)
            .WithOne(y => y.Analysis)
            .HasForeignKey<AnalysisResponse>(y => y.IdAnalysis)
            .IsRequired();
    }
}