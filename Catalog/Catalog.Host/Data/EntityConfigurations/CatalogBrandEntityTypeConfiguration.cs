using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public class CatalogBrandEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder
            .ToTable("CatalogBrand");

        builder
            .HasKey(cb => cb.Id);

        builder
            .Property(cb => cb.Id)
            .UseHiLo("catalog_brand_hilo")
            .IsRequired();

        builder
            .Property(cb => cb.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasData(new List<CatalogBrand>()
        {
            new CatalogBrand() { Id = 1, Name = "A4Tech" },
            new CatalogBrand() { Id = 2, Name = "AMD" },
            new CatalogBrand() { Id = 3, Name = "Aorus" },
            new CatalogBrand() { Id = 4, Name = "Apple" },
            new CatalogBrand() { Id = 5, Name = "Asus" },
            new CatalogBrand() { Id = 6, Name = "Bloody" },
            new CatalogBrand() { Id = 7, Name = "Edifier" },
            new CatalogBrand() { Id = 8, Name = "Gigabyte" },
            new CatalogBrand() { Id = 9, Name = "Hator" },
            new CatalogBrand() { Id = 10, Name = "Honor" },
            new CatalogBrand() { Id = 11, Name = "Huawei" },
            new CatalogBrand() { Id = 12, Name = "HyperX" },
            new CatalogBrand() { Id = 13, Name = "Intel" },
            new CatalogBrand() { Id = 14, Name = "Keychron" },
            new CatalogBrand() { Id = 15, Name = "Kingston" },
            new CatalogBrand() { Id = 16, Name = "Logitech" },
            new CatalogBrand() { Id = 17, Name = "MSI" },
            new CatalogBrand() { Id = 18, Name = "Razer" },
            new CatalogBrand() { Id = 19, Name = "Samsung" },
            new CatalogBrand() { Id = 20, Name = "Seagate" },
            new CatalogBrand() { Id = 21, Name = "Sony" },
            new CatalogBrand() { Id = 22, Name = "SteelSeries" },
            new CatalogBrand() { Id = 23, Name = "Varmilo" },
            new CatalogBrand() { Id = 24, Name = "Western Digital" },
        });
    }
}
