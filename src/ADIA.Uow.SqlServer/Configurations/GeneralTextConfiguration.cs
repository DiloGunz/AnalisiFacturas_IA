using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADIA.Uow.SqlServer.Configurations;

public class GeneralTextConfiguration : IEntityTypeConfiguration<Model.Domain.Entities.GeneralText>
{
    public void Configure(EntityTypeBuilder<Model.Domain.Entities.GeneralText> b)
    {
        b.Property(x => x.Summary).IsRequired();
        b.Property(x => x.Description).IsRequired();
        b.Property(x => x.Sentiment).IsRequired();
    }
}