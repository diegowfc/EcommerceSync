using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Citext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Garante a extensão citext (para o snapshot do EF)
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            // (Opcional, mas útil se o banco não tiver a extensão ainda)
            migrationBuilder.Sql(@"CREATE EXTENSION IF NOT EXISTS citext;");

            // 2) Converte a coluna para citext
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "tab_user",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            // 3) Índice único (idempotente)
            migrationBuilder.Sql(@"
DO $$
BEGIN
    -- cria se não existir
    IF NOT EXISTS (
        SELECT 1 FROM pg_indexes
        WHERE schemaname = 'public'
          AND tablename  = 'tab_user'
          AND indexname  = 'IX_tab_user_Email'
    ) THEN
        CREATE UNIQUE INDEX ""IX_tab_user_Email"" ON public.tab_user (""Email"");
    ELSE
        -- se existir e não for único, recria como único
        IF NOT EXISTS (
            SELECT 1
            FROM pg_class c
            JOIN pg_index i ON c.oid = i.indexrelid
            WHERE c.relname = 'IX_tab_user_Email'
              AND i.indisunique = true
        ) THEN
            DROP INDEX IF EXISTS ""IX_tab_user_Email"";
            CREATE UNIQUE INDEX ""IX_tab_user_Email"" ON public.tab_user (""Email"");
        END IF;
    END IF;
END$$;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // remove índice (se existir)
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_tab_user_Email"";");

            // volta a coluna para text
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "tab_user",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext");

            // remove a anotação do snapshot
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");
        }
    }
}
