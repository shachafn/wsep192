using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class iniil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingBags",
                columns: table => new
                {
                    BagGuid = table.Column<Guid>(nullable: false),
                    UserGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingBags", x => x.BagGuid);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    ShopState = table.Column<int>(nullable: false),
                    ShopName = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    cartGuid = table.Column<Guid>(nullable: false),
                    UserGuid = table.Column<Guid>(nullable: false),
                    ShopGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Carts = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.cartGuid);
                    table.ForeignKey(
                        name: "FK_Carts_ShoppingBags_Carts",
                        column: x => x.Carts,
                        principalTable: "ShoppingBags",
                        principalColumn: "BagGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopOwners",
                columns: table => new
                {
                    OwnerGuid = table.Column<Guid>(nullable: false),
                    OwnerBaseUserGuid = table.Column<Guid>(nullable: false),
                    AppointerGuid = table.Column<Guid>(nullable: false),
                    ShopGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ShopDAOGuid = table.Column<Guid>(nullable: true),
                    ShopDAOGuid1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopOwners", x => x.OwnerGuid);
                    table.ForeignKey(
                        name: "FK_ShopOwners_Shops_ShopDAOGuid",
                        column: x => x.ShopDAOGuid,
                        principalTable: "Shops",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopOwners_Shops_ShopDAOGuid1",
                        column: x => x.ShopDAOGuid1,
                        principalTable: "Shops",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductGuid = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ShopDAOGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopProducts_Products_ProductGuid",
                        column: x => x.ProductGuid,
                        principalTable: "Products",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopProducts_Shops_ShopDAOGuid",
                        column: x => x.ShopDAOGuid,
                        principalTable: "Shops",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartRecords",
                columns: table => new
                {
                    CartGuid = table.Column<Guid>(nullable: false),
                    ProductGuid = table.Column<Guid>(nullable: false),
                    PurchasedQuantity = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CartRecords = table.Column<Guid>(nullable: true),
                    ShopDAOGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRecords", x => new { x.CartGuid, x.ProductGuid });
                    table.ForeignKey(
                        name: "FK_CartRecords_Carts_CartRecords",
                        column: x => x.CartRecords,
                        principalTable: "Carts",
                        principalColumn: "cartGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartRecords_Shops_ShopDAOGuid",
                        column: x => x.ShopDAOGuid,
                        principalTable: "Shops",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartRecords_CartRecords",
                table: "CartRecords",
                column: "CartRecords");

            migrationBuilder.CreateIndex(
                name: "IX_CartRecords_ShopDAOGuid",
                table: "CartRecords",
                column: "ShopDAOGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_Carts",
                table: "Carts",
                column: "Carts");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShopOwners_ShopDAOGuid",
                table: "ShopOwners",
                column: "ShopDAOGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ShopOwners_ShopDAOGuid1",
                table: "ShopOwners",
                column: "ShopDAOGuid1");

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_ProductGuid",
                table: "ShopProducts",
                column: "ProductGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_ShopDAOGuid",
                table: "ShopProducts",
                column: "ShopDAOGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ShopName",
                table: "Shops",
                column: "ShopName",
                unique: true,
                filter: "[ShopName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartRecords");

            migrationBuilder.DropTable(
                name: "ShopOwners");

            migrationBuilder.DropTable(
                name: "ShopProducts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "ShoppingBags");
        }
    }
}
