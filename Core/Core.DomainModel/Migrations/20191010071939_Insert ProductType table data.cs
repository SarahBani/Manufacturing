using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.DomainModel.Migrations
{
    public partial class InsertProductTypetabledata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = @"
                INSERT INTO dbo.ProductType(ProductTypeID, Name, Width, StackedCount)
                SELECT 1, 'PhotoBook', 19, 1
                UNION
                SELECT 2, 'Calendar', 10, 1
                UNION
                SELECT 3, 'Canvas', 16, 1
                UNION
                SELECT 4, 'Cards', 4.7, 1
                UNION
                SELECT 5, 'Mug', 94, 4
                ";
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
