using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Host.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_brand_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "CatalogBrand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBrand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    AvailableStock = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Warranty = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PictureFileName = table.Column<string>(type: "text", nullable: true),
                    CatalogBrandId = table.Column<int>(type: "integer", nullable: false),
                    CatalogTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalog_CatalogBrand_CatalogBrandId",
                        column: x => x.CatalogBrandId,
                        principalTable: "CatalogBrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catalog_CatalogType_CatalogTypeId",
                        column: x => x.CatalogTypeId,
                        principalTable: "CatalogType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CatalogBrand",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "A4Tech" },
                    { 2, "AMD" },
                    { 3, "Aorus" },
                    { 4, "Apple" },
                    { 5, "Asus" },
                    { 6, "Bloody" },
                    { 7, "Edifier" },
                    { 8, "Gigabyte" },
                    { 9, "Hator" },
                    { 10, "Honor" },
                    { 11, "Huawei" },
                    { 12, "HyperX" },
                    { 13, "Intel" },
                    { 14, "Keychron" },
                    { 15, "Kingston" },
                    { 16, "Logitech" },
                    { 17, "MSI" },
                    { 18, "Razer" },
                    { 19, "Samsung" },
                    { 20, "Seagate" },
                    { 21, "Sony" },
                    { 22, "SteelSeries" },
                    { 23, "Varmilo" },
                    { 24, "Western Digital" }
                });

            migrationBuilder.InsertData(
                table: "CatalogType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Computer Case" },
                    { 2, "Desk Mount" },
                    { 3, "Gamepad" },
                    { 4, "Graphics Card (GPU)" },
                    { 5, "Hard-Disk Drive (HDD)" },
                    { 6, "Headphones" },
                    { 7, "Keyboard" },
                    { 8, "Laptop" },
                    { 9, "Memory (RAM)" },
                    { 10, "Microphone" },
                    { 11, "Monitor" },
                    { 12, "Motherboard" },
                    { 13, "Mouse" },
                    { 14, "MousePad" },
                    { 15, "Processor (CPU)" },
                    { 16, "Power Supply Unit (PSU)" },
                    { 17, "Solid-State Drive (SSD)" },
                    { 18, "SmartPhone" },
                    { 19, "SmartWatch" },
                    { 20, "Speakers" },
                    { 21, "Subwoofer" },
                    { 22, "Tablet" },
                    { 23, "Web Camera" },
                    { 24, "Wrist Rest" }
                });

            migrationBuilder.InsertData(
                table: "Catalog",
                columns: new[] { "Id", "AvailableStock", "CatalogBrandId", "CatalogTypeId", "Description", "Name", "PictureFileName", "Price", "Warranty" },
                values: new object[,]
                {
                    { 1, 1, 7, 20, "R1700BTs Active Bluetooth Bookshelf Speakers - 2.0 Wireless Near Field Studio Monitor Speaker - 66w RMS with Subwoofer Line Out. BLUETOOTH 5.0 WITH QUALCOMM APTX HD. SUB OUT. NATURAL SOUND REPRODUCTION. UPDATED WIRELESS REMOTE. 2 YEAR WARRANTY.", "R1700BTs", "r1700bts.webp", 4599.00m, 12 },
                    { 2, 6, 16, 23, "C270 HD Webcam, HD 720p, Widescreen HD Video Calling, HD Light Correction, Noise-Reducing Mic, For Skype, FaceTime, Hangouts, WebEx, PC/Mac/Laptop/Macbook/Tablet - Black. The C270 HD Webcam gives you sharp, smooth conference calls (720p/30fps) in a widescreen format. Automatic light correction shows you in lifelike, natural colors.", "C270", "c270hd.webp", 1299.00m, 12 },
                    { 3, 5, 14, 7, "K3 Version 2 - 75% Layot 84 Keys Ultra-Slim Wireless Bluetooth/USB Wired Mechanical Keyboard with RGB LED Backlit, Low-Profile Keychron Optical Hot-Swappable Brown Switches, Aluminium Frame, Compatible with Mac & Windows.", "K3 Version 2", "k3version2.webp", 2990.00m, 24 },
                    { 4, 5, 14, 7, "Q1 - QMK Custom Mechanical Keyboard - Fully Assembled - Wired USB - Gateron Phantom Brown - Navy Blue. A fully customizable 75% layout mechanical keyboard packed with all premium features and unlimited possibilities.", "Q1", "q1qmk.webp", 4700.00m, 24 },
                    { 5, 1, 14, 7, "Q2 - QMK Custom Mechanical Keyboard - Fully Assembled - Wired USB - Gateron G Pro Brown - Carbon Black. The Q2 is a fully customizable mechanical keyboard with a compact layout that pushes your typing experience to the next level. With a 65% layout, full metal body, double-gasket design, the Q2 is designed for a personalized experience and premium typing comfort.", "Q2", "q2qmk.webp", 4800.00m, 24 },
                    { 6, 2, 16, 7, "K360 Wireless USB Desktop Keyboard — Compact Full Keyboard, 3-Year Battery Life (Glossy Black). K360 is ready when you are. This compact wireless keyboard is ideal for narrower built and allows you to perform even in constricted workspaces. Equipped with a number pad and 12 easy access keys, you can be more productive - at home or at work.", "K360", "k360.webp", 1399.00m, 24 },
                    { 7, 7, 16, 7, "MX Keys Advanced Wireless Illuminated Keyboard, Tactile Responsive Typing, Backlighting, Bluetooth, USB-C, Apple macOS, Microsoft Windows, Linux, iOS, Android, Metal Build - Graphite. Logitech’s most advanced typing experience yet. MX Keys combines crafted keys with smart illumination and a remarkably solid build.", "MX Keys Advanced", "mxkeys.webp", 2999.00m, 36 },
                    { 8, 2, 18, 7, "BlackWidow V3 Tenkeyless TKL Mechanical Gaming Keyboard: Green Mechanical Switches - Tactile & Clicky - Chroma RGB Lighting - Compact Form Factor - Programmable Macros. Compact gaming keyboard featuring Razer Mechanical Switches, customizable lighting powered by Razer Chroma RGB, and aluminum construction for amazing gaming experience.", "BlackWidow V3", "blackwidowv3tkl.webp", 2799.00m, 24 },
                    { 9, 4, 23, 7, "Miya68-C Summit Series Wired Mechanical Keyboard. Dye-sub on the top printing.", "Miya68-C Summit Series EC V2 Daisy", "miya68csummitecswitchv2.webp", 5900.00m, 36 },
                    { 10, 2, 23, 7, "MA87 Lovebirds-You Series Wired Mechanical Keyboard. Five-sides dye-sub printing.", "MA87 Lovebirds-You Series EC V2 Daisy", "ma87lovebirdsyou.webp", 6100.00m, 36 },
                    { 11, 4, 23, 7, "MA87M V2 Summit Series R2 EC V2 Daisy Wired USB Mechanical Keyboard. Dye-sub on the top printing.", "MA87M V2 Summit Series R2 EC V2 Daisy", "ma87mv2summitr2ecv2daisy.webp", 6000.00m, 36 },
                    { 12, 1, 23, 7, "VA104S Phoenix Series Wired Mechanical Keyboard. EC V2 Daisy. Five-sides dye-sub printing.", "VA104S Phoenix Series EC V2 Daisy", "va104sphoenix.webp", 6300.00m, 36 },
                    { 13, 3, 23, 7, "VBM108V2 Crane of Lure Series EC V2 Daisy Wired Mechanical Keyboard. Dye-sub on the top printing.", "VBM108V2 Crane of Lure Series EC V2 Daisy", "vbm108v2craneoflure.webp", 6800.00m, 36 },
                    { 14, 3, 16, 13, "G305 LIGHTSPEED Wireless Gaming Mouse, Hero 12K Sensor, 12,000 DPI, Lightweight, 6 Programmable Buttons, 250h Battery Life, On-Board Memory, PC/Mac - Blue. LIGHTSPEED wireless gaming mouse designed for serious performance with latest technology innovations. Impressive 250-hour battery life. Now in a variety of vibrant colors.", "G305 LIGHTSPEED", "g305lightspeedblue.webp", 1899.00m, 12 },
                    { 15, 4, 16, 13, "G502 SE HERO High Performance RGB Gaming Mouse with 11 Programmable Buttons USB Black/White. G502 SE HERO features an advanced optical sensor for maximum tracking accuracy, customizable RGB lighting, custom game profiles, from 200 up to 25,600 DPI, and repositionable weights.", "G502 SE HERO", "g502sehero.webp", 1699.00m, 36 },
                    { 16, 4, 16, 8, "MX Master 3 Advanced Wireless Mouse, Ultrafast Scrolling, Ergonomic, 4000 DPI, Customization, USB-C, Bluetooth, USB, Apple Mac, Microsoft PC Windows, Linux, iPad - Graphite. An advanced, precise mouse designed for creatives and engineered for coders, featuring a side wheel for extra comfort and a natural feel.", "MX Master 3 Advanced", "mxmaster3.webp", 2899.00m, 36 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogBrandId",
                table: "Catalog",
                column: "CatalogBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogTypeId",
                table: "Catalog",
                column: "CatalogTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catalog");

            migrationBuilder.DropTable(
                name: "CatalogBrand");

            migrationBuilder.DropTable(
                name: "CatalogType");

            migrationBuilder.DropSequence(
                name: "catalog_brand_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_type_hilo");
        }
    }
}
