using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public class CatalogItemEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder
            .ToTable("Catalog");

        builder
            .Property(ci => ci.Id)
            .UseHiLo("catalog_hilo")
            .IsRequired();

        builder
            .Property(ci => ci.Name)
            .IsRequired(true)
            .HasMaxLength(350);

        builder
            .Property(ci => ci.AvailableStock)
            .IsRequired(true);

        builder
            .Property(ci => ci.Price)
            .IsRequired(true);

        builder
            .Property(ci => ci.Warranty)
            .IsRequired(true);

        builder
            .Property(ci => ci.Description)
            .IsRequired(false);

        builder
            .Property(ci => ci.PictureFileName)
            .IsRequired(false);

        builder
            .HasOne(ci => ci.CatalogBrand)
            .WithMany()
            .HasForeignKey(ci => ci.CatalogBrandId);

        builder
            .HasOne(ci => ci.CatalogType)
            .WithMany()
            .HasForeignKey(ci => ci.CatalogTypeId);

        builder
            .HasData(new List<CatalogItem>()
        {
            new CatalogItem
            {
                Id = 1,
                CatalogBrandId = 7,
                CatalogTypeId = 20,
                AvailableStock = 1,
                Description = "R1700BTs Active Bluetooth Bookshelf Speakers - 2.0 Wireless Near Field " +
                "Studio Monitor Speaker - 66w RMS with Subwoofer Line Out. BLUETOOTH 5.0 WITH QUALCOMM " +
                "APTX HD. SUB OUT. NATURAL SOUND REPRODUCTION. UPDATED WIRELESS REMOTE. 2 YEAR WARRANTY.",
                Name = "R1700BTs",
                Price = 4599.00M,
                PictureFileName = "r1700bts.webp",
                Warranty = 12,
            },
            new CatalogItem
            {
                Id = 2,
                CatalogBrandId = 16,
                CatalogTypeId = 23,
                AvailableStock = 6,
                Description = "C270 HD Webcam, HD 720p, Widescreen HD Video Calling, HD Light Correction, " +
                "Noise-Reducing Mic, For Skype, FaceTime, Hangouts, WebEx, PC/Mac/Laptop/Macbook/Tablet - " +
                "Black. The C270 HD Webcam gives you sharp, smooth conference calls (720p/30fps) in a " +
                "widescreen format. Automatic light correction shows you in lifelike, natural colors.",
                Name = "C270",
                Price = 1299.00M,
                PictureFileName = "c270hd.webp",
                Warranty = 12,
            },
            new CatalogItem
            {
                Id = 3,
                CatalogBrandId = 14,
                CatalogTypeId = 7,
                AvailableStock = 5,
                Description = "K3 Version 2 - 75% Layot 84 Keys Ultra-Slim Wireless Bluetooth/USB Wired " +
                "Mechanical Keyboard with RGB LED Backlit, Low-Profile Keychron Optical Hot-Swappable " +
                "Brown Switches, Aluminium Frame, Compatible with Mac & Windows.",
                Name = "K3 Version 2",
                Price = 2990.00M,
                PictureFileName = "k3version2.webp",
                Warranty = 24,
            },
            new CatalogItem
            {
                Id = 4,
                CatalogBrandId = 14,
                CatalogTypeId = 7,
                AvailableStock = 5,
                Description = "Q1 - QMK Custom Mechanical Keyboard - Fully Assembled - Wired USB - Gateron " +
                "Phantom Brown - Navy Blue. A fully customizable 75% layout mechanical keyboard packed with all premium " +
                "features and unlimited possibilities.",
                Name = "Q1",
                Price = 4700.00M,
                PictureFileName = "q1qmk.webp",
                Warranty = 24,
            },
            new CatalogItem
            {
                Id = 5,
                CatalogBrandId = 14,
                CatalogTypeId = 7,
                AvailableStock = 1,
                Description = "Q2 - QMK Custom Mechanical Keyboard - Fully Assembled - Wired USB - Gateron G Pro Brown - " +
                "Carbon Black. The Q2 is a fully customizable mechanical keyboard with a compact layout that pushes your " +
                "typing experience to the next level. With a 65% layout, full metal body, double-gasket design, the Q2 is " +
                "designed for a personalized experience and premium typing comfort.",
                Name = "Q2",
                Price = 4800.00M,
                PictureFileName = "q2qmk.webp",
                Warranty = 24,
            },
            new CatalogItem
            {
                Id = 6,
                CatalogBrandId = 16,
                CatalogTypeId = 7,
                AvailableStock = 2,
                Description = "K360 Wireless USB Desktop Keyboard — Compact Full Keyboard, 3-Year Battery Life (Glossy " +
                "Black). K360 is ready when you are. This compact wireless keyboard is ideal for narrower built and allows " +
                "you to perform even in constricted workspaces. Equipped with a number pad and 12 easy access keys, you can " +
                "be more productive - at home or at work.",
                Name = "K360",
                Price = 1399.00M,
                PictureFileName = "k360.webp",
                Warranty = 24,
            },
            new CatalogItem
            {
                Id = 7,
                CatalogBrandId = 16,
                CatalogTypeId = 7,
                AvailableStock = 7,
                Description = "MX Keys Advanced Wireless Illuminated Keyboard, Tactile Responsive Typing, " +
                "Backlighting, Bluetooth, USB-C, Apple macOS, Microsoft Windows, Linux, iOS, Android, Metal " +
                "Build - Graphite. Logitech’s most advanced typing experience yet. MX Keys combines crafted keys " +
                "with smart illumination and a remarkably solid build.",
                Name = "MX Keys Advanced",
                Price = 2999.00M,
                PictureFileName = "mxkeys.webp",
                Warranty = 36,
            },
            new CatalogItem
            {
                Id = 8,
                CatalogBrandId = 18,
                CatalogTypeId = 7,
                AvailableStock = 2,
                Description = "BlackWidow V3 Tenkeyless TKL Mechanical Gaming Keyboard: Green Mechanical Switches " +
                "- Tactile & Clicky - Chroma RGB Lighting - Compact Form Factor - Programmable Macros. Compact gaming " +
                "keyboard featuring Razer Mechanical Switches, customizable lighting powered by Razer Chroma RGB, and " +
                "aluminum construction for amazing gaming experience.",
                Name = "BlackWidow V3",
                Price = 2799.00M,
                PictureFileName = "blackwidowv3tkl.webp",
                Warranty = 24,
            },
            new CatalogItem
            {
                Id = 9,
                CatalogBrandId = 23,
                CatalogTypeId = 7,
                AvailableStock = 4,
                Description = "Miya68-C Summit Series Wired Mechanical Keyboard. Dye-sub on the top printing.",
                Name = "Miya68-C Summit Series EC V2 Daisy",
                Price = 5900.00M,
                PictureFileName = "miya68csummitecswitchv2.webp",
                Warranty = 36,
            },
            new CatalogItem
            {
                Id = 10,
                CatalogBrandId = 23,
                CatalogTypeId = 7,
                AvailableStock = 2,
                Description = "MA87 Lovebirds-You Series Wired Mechanical Keyboard. Five-sides dye-sub printing.",
                Name = "MA87 Lovebirds-You Series EC V2 Daisy",
                Price = 6100.00M,
                PictureFileName = "ma87lovebirdsyou.webp",
                Warranty = 36,
            },
            new CatalogItem
            {
                Id = 11,
                CatalogBrandId = 23,
                CatalogTypeId = 7,
                AvailableStock = 4,
                Description = "MA87M V2 Summit Series R2 EC V2 Daisy Wired USB Mechanical Keyboard. Dye-sub on the top printing.",
                Name = "MA87M V2 Summit Series R2 EC V2 Daisy",
                Price = 6000.00M,
                PictureFileName = "ma87mv2summitr2ecv2daisy.webp",
                Warranty = 36,
            },
            new CatalogItem
            {
                Id = 12,
                CatalogBrandId = 23,
                CatalogTypeId = 7,
                AvailableStock = 1,
                Description = "VA104S Phoenix Series Wired Mechanical Keyboard. EC V2 Daisy. Five-sides dye-sub printing.",
                Name = "VA104S Phoenix Series EC V2 Daisy",
                Price = 6300.00M,
                PictureFileName = "va104sphoenix.webp",
                Warranty = 36,
            },
            new CatalogItem
            {
                Id = 13,
                CatalogBrandId = 23,
                CatalogTypeId = 7,
                AvailableStock = 3,
                Description = "VBM108V2 Crane of Lure Series EC V2 Daisy Wired Mechanical Keyboard. Dye-sub on the top printing.",
                Name = "VBM108V2 Crane of Lure Series EC V2 Daisy",
                Price = 6800.00M,
                PictureFileName = "vbm108v2craneoflure.webp",
                Warranty = 36,
            },
            new CatalogItem
            {
                Id = 14,
                CatalogBrandId = 16,
                CatalogTypeId = 13,
                AvailableStock = 3,
                Description = "G305 LIGHTSPEED Wireless Gaming Mouse, Hero 12K Sensor, 12,000 DPI, " +
                "Lightweight, 6 Programmable Buttons, 250h Battery Life, On-Board Memory, PC/Mac - Blue. " +
                "LIGHTSPEED wireless gaming mouse designed for serious performance with latest technology " +
                "innovations. Impressive 250-hour battery life. Now in a variety of vibrant colors.",
                Name = "G305 LIGHTSPEED",
                Price = 1899.00M,
                PictureFileName = "g305lightspeedblue.webp",
                Warranty = 12,
            },
            new CatalogItem
            {
                Id = 15,
                CatalogBrandId = 16,
                CatalogTypeId = 13,
                AvailableStock = 4,
                Description = "G502 SE HERO High Performance RGB Gaming Mouse with 11 Programmable " +
                "Buttons USB Black/White. G502 SE HERO features an advanced optical sensor for maximum " +
                "tracking accuracy, customizable RGB lighting, custom game profiles, from 200 up to 25,600 " +
                "DPI, and repositionable weights.",
                Name = "G502 SE HERO",
                Price = 1699.00M,
                PictureFileName = "g502sehero.webp",
                Warranty = 36,
            },
            new CatalogItem
            {
                Id = 16,
                CatalogBrandId = 16,
                CatalogTypeId = 8,
                AvailableStock = 4,
                Description = "MX Master 3 Advanced Wireless Mouse, Ultrafast Scrolling, Ergonomic, 4000 DPI, " +
                "Customization, USB-C, Bluetooth, USB, Apple Mac, Microsoft PC Windows, Linux, iPad - Graphite. " +
                "An advanced, precise mouse designed for creatives and engineered for coders, featuring a side " +
                "wheel for extra comfort and a natural feel.",
                Name = "MX Master 3 Advanced",
                Price = 2899.00M,
                PictureFileName = "mxmaster3.webp",
                Warranty = 36,
            },
        });
    }
}
