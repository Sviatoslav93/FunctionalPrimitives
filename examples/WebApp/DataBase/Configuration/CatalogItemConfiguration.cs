using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Domain.Catalog;

namespace WebApp.DataBase.Configuration;

public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.Property(i => i.Id).ValueGeneratedOnAdd();

        builder.Property(i => i.Sku)
            .HasColumnName("Sku");

        builder.HasIndex(x => x.Sku).IsUnique();

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(CatalogItemSchemaConstants.NameMaxLenght);

        builder.Property(i => i.Description)
            .IsRequired(false)
            .HasMaxLength(CatalogItemSchemaConstants.DescriptionMaxLength);
    }
}
