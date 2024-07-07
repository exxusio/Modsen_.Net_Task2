using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Guidtoid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderGuid",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductGuid",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserGuid",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryGuid",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleGuid",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoleGuid",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleGuid",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CategoryGuid",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryGuid",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserGuid",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.RenameColumn(
                name: "ProductGuid",
                table: "OrderItems",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "OrderGuid",
                table: "OrderItems",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductGuid",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductId");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Categories",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "RoleGuid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "Guid");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                newName: "IX_Users_RoleGuid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Products",
                newName: "CategoryGuid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "Guid");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryGuid");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "Guid");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                newName: "IX_Orders_UserGuid");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItems",
                newName: "ProductGuid");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderItems",
                newName: "OrderGuid");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductGuid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderGuid",
                table: "OrderItems",
                column: "OrderGuid",
                principalTable: "Orders",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductGuid",
                table: "OrderItems",
                column: "ProductGuid",
                principalTable: "Products",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserGuid",
                table: "Orders",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryGuid",
                table: "Products",
                column: "CategoryGuid",
                principalTable: "Categories",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleGuid",
                table: "Users",
                column: "RoleGuid",
                principalTable: "Roles",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
