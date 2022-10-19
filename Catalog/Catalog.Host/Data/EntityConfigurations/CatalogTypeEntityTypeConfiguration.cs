using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public class CatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder
            .ToTable("CatalogType");

        builder
            .HasKey(ct => ct.Id);

        builder
            .Property(ct => ct.Id)
            .UseHiLo("catalog_type_hilo")
            .IsRequired();

        builder
            .Property(ct => ct.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasData(new List<CatalogType>()
        {
            new CatalogType() { Id = 1, Name = "Computer Case" },
            new CatalogType() { Id = 2, Name = "Desk Mount" },
            new CatalogType() { Id = 3, Name = "Gamepad" },
            new CatalogType() { Id = 4, Name = "Graphics Card (GPU)" },
            new CatalogType() { Id = 5, Name = "Hard-Disk Drive (HDD)" },
            new CatalogType() { Id = 6, Name = "Headphones" },
            new CatalogType() { Id = 7, Name = "Keyboard" },
            new CatalogType() { Id = 8, Name = "Laptop" },
            new CatalogType() { Id = 9, Name = "Memory (RAM)" },
            new CatalogType() { Id = 10, Name = "Microphone" },
            new CatalogType() { Id = 11, Name = "Monitor" },
            new CatalogType() { Id = 12, Name = "Motherboard" },
            new CatalogType() { Id = 13, Name = "Mouse" },
            new CatalogType() { Id = 14, Name = "MousePad" },
            new CatalogType() { Id = 15, Name = "Processor (CPU)" },
            new CatalogType() { Id = 16, Name = "Power Supply Unit (PSU)" },
            new CatalogType() { Id = 17, Name = "Solid-State Drive (SSD)" },
            new CatalogType() { Id = 18, Name = "SmartPhone" },
            new CatalogType() { Id = 19, Name = "SmartWatch" },
            new CatalogType() { Id = 20, Name = "Speakers" },
            new CatalogType() { Id = 21, Name = "Subwoofer" },
            new CatalogType() { Id = 22, Name = "Tablet" },
            new CatalogType() { Id = 23, Name = "Web Camera" },
            new CatalogType() { Id = 24, Name = "Wrist Rest" },
        });
    }
}
