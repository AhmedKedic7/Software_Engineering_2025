using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeStore.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Brands__DAD4F3BE33E296C3", x => x.BrandID);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ColorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Colors__8DA7676D8C5246BF", x => x.ColorID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<short>(type: "smallint", nullable: false),
                    IsLogedin = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC18488A49", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BrandID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ColorID = table.Column<int>(type: "int", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: true),
                    IsLast = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LockedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => new { x.ProductID, x.Version });
                    table.ForeignKey(
                        name: "FK__Products__BrandI__286302EC",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "BrandID");
                    table.ForeignKey(
                        name: "FK__Products__ColorI__2A4B4B5E",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ColorID");
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    address_line = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Addresse__091C2A1B117E06DB", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK__Addresses__UserI__31EC6D26",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Carts__51BCD797AF854046", x => x.CartID);
                    table.ForeignKey(
                        name: "FK__Carts__UserID__4D94879B",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Images__7516F4EC3A7AD4DB", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_Images_Products",
                        columns: x => new { x.ProductID, x.Version },
                        principalTable: "Products",
                        principalColumns: new[] { "ProductID", "Version" });
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "money", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    AddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__C3905BAF0C933DBA", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "AddressID");
                    table.ForeignKey(
                        name: "FK__Orders__UserID__35BCFE0A",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CartItem__488B0B2A6056BEC8", x => x.CartItemID);
                    table.ForeignKey(
                        name: "FK_CartItem_Products",
                        columns: x => new { x.ProductID, x.Version },
                        principalTable: "Products",
                        principalColumns: new[] { "ProductID", "Version" });
                    table.ForeignKey(
                        name: "FK__CartItem__CartID__5CD6CB2B",
                        column: x => x.CartID,
                        principalTable: "Carts",
                        principalColumn: "CartID");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderIte__57ED06A1F4D9C7CF", x => x.OrderItemID);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products",
                        columns: x => new { x.ProductID, x.Version },
                        principalTable: "Products",
                        principalColumns: new[] { "ProductID", "Version" });
                    table.ForeignKey(
                        name: "FK__OrderItem__Order__49C3F6B7",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserID",
                table: "Addresses",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartID",
                table: "CartItem",
                column: "CartID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductID_Version",
                table: "CartItem",
                columns: new[] { "ProductID", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserID",
                table: "Carts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductID_Version",
                table: "Images",
                columns: new[] { "ProductID", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderID",
                table: "OrderItems",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductID_Version",
                table: "OrderItems",
                columns: new[] { "ProductID", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressID",
                table: "Orders",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandID",
                table: "Products",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ColorID",
                table: "Products",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "UQ_Product_Version",
                table: "Products",
                columns: new[] { "ProductID", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
