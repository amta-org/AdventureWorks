using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Catalog.API.Infrastructure.Migrations;

[DbContext(typeof(CatalogContext))]
[Migration("20240528000001_AddProductReviews")]
public class ProductReviewsMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateSequence(
            name: "product_review_hilo",
            incrementBy: 10);

        migrationBuilder.CreateTable(
            name: "ProductReviews",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false),
                UserId = table.Column<string>(nullable: false),
                CatalogItemId = table.Column<int>(nullable: false),
                Rating = table.Column<int>(nullable: false),
                ReviewText = table.Column<string>(maxLength: 1000, nullable: true),
                CreatedAt = table.Column<DateTime>(nullable: false),
                UpdatedAt = table.Column<DateTime>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductReviews", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProductReviews_Catalog_CatalogItemId",
                    column: x => x.CatalogItemId,
                    principalTable: "Catalog",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProductReviews_CatalogItemId",
            table: "ProductReviews",
            column: "CatalogItemId");

        // Add AverageRating column to Catalog table
        migrationBuilder.AddColumn<double>(
            name: "AverageRating",
            table: "Catalog",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "ProductReviews");
        migrationBuilder.DropSequence(name: "product_review_hilo");
        migrationBuilder.DropColumn(name: "AverageRating", table: "Catalog");
    }
}