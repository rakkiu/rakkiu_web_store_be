using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCategoryIdToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop the foreign key constraint
            migrationBuilder.Sql(@"
                ALTER TABLE ""ProductCategories"" 
                DROP CONSTRAINT ""FK_ProductCategories_Categories_CategoryId"";
            ");

            // Step 2: Drop the index
            migrationBuilder.Sql(@"
                DROP INDEX ""IX_ProductCategories_CategoryId"";
            ");

            // Step 3: Alter the foreign key column
            migrationBuilder.Sql(@"
                ALTER TABLE ""ProductCategories"" 
                ALTER COLUMN ""CategoryId"" TYPE bigint 
                USING (0)::bigint;
            ");

            // Step 4: Alter the primary key column
            migrationBuilder.Sql(@"
                ALTER TABLE ""Categories"" 
                ALTER COLUMN ""Id"" TYPE bigint 
                USING (0)::bigint;
            ");
            
            // Step 5: Add identity generation for Categories.Id
            migrationBuilder.Sql(@"
                CREATE SEQUENCE IF NOT EXISTS ""Categories_Id_seq"";
                ALTER TABLE ""Categories"" 
                ALTER COLUMN ""Id"" SET DEFAULT nextval('""Categories_Id_seq""');
                ALTER SEQUENCE ""Categories_Id_seq"" OWNED BY ""Categories"".""Id"";
            ");

            // Step 6: Recreate the index
            migrationBuilder.Sql(@"
                CREATE INDEX ""IX_ProductCategories_CategoryId"" ON ""ProductCategories"" (""CategoryId"");
            ");

            // Step 7: Recreate the foreign key constraint
            migrationBuilder.Sql(@"
                ALTER TABLE ""ProductCategories"" 
                ADD CONSTRAINT ""FK_ProductCategories_Categories_CategoryId"" 
                FOREIGN KEY (""CategoryId"") REFERENCES ""Categories"" (""Id"") ON DELETE CASCADE;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove identity generation
            migrationBuilder.Sql(@"
                ALTER TABLE ""Categories"" 
                ALTER COLUMN ""Id"" DROP DEFAULT;
                DROP SEQUENCE IF EXISTS ""Categories_Id_seq"";
            ");
            
            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "ProductCategories",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
