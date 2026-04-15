/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Third demo blog post - EF Core migrations in production.
          Idempotent: guarded by SLUG.
**/
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.BLOG_POSTS WHERE SLUG = N'ef-core-migrations-in-production')
BEGIN
    DECLARE @PostId INT;

    INSERT INTO dbo.BLOG_POSTS
        (TITLE, SLUG, SUMMARY, TAGS, IS_PUBLISHED, PUBLISHED_DATE, VIEW_COUNT, CREATED_BY, CREATED_DATE)
    VALUES
        (N'EF Core migrations in production - what nobody tells you',
         N'ef-core-migrations-in-production',
         N'The migration commands you already know, plus the ones you need when the first deploy fails and you are staring at a locked database at 2am.',
         N'.NET, EF Core, SQL Server, Migrations',
         1,
         GETDATE(),
         0,
         N'admin',
         GETDATE());

    SET @PostId = SCOPE_IDENTITY();

    ;WITH blocks(sort_, type_, content_, lang_, font_, size_, style_, align_) AS (
        SELECT  10, N'Heading',
                N'The happy path',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 20, N'Text',
                N'In development you run two commands and move on. Most tutorials end here.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 30, N'Code',
                N'dotnet ef migrations add AddBlogTables
dotnet ef database update',
                N'bash', NULL, NULL, NULL, NULL
        UNION ALL SELECT 40, N'Heading',
                N'Production is not development',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 50, N'Text',
                N'On a live server you usually cannot run ef tool commands (no SDK, no access, or CI/CD in the middle). Generate an idempotent SQL script instead and hand it to DBAs or run it from your pipeline.',
                NULL, N'Geist, sans-serif', N'1.02rem', NULL, N'justify'
        UNION ALL SELECT 60, N'Code',
                N'# from InitialCreate up to the latest migration, safe to re-run
dotnet ef migrations script --idempotent --output migrate.sql

# or only the delta between two specific migrations
dotnet ef migrations script AddBlogTables AddBlogViewCounts \
    --idempotent --output 2026-04-16_delta.sql',
                N'bash', NULL, NULL, NULL, NULL
        UNION ALL SELECT 70, N'Heading',
                N'The three rules that save you',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 80, N'Text',
                N'1. Never edit a migration that has already been applied to any shared environment. Add a new one instead.\n2. Every migration must be reversible - test the Down method locally before you merge.\n3. Separate schema changes from data fixes. Mixing them makes a half-failed deploy almost impossible to recover.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 90, N'Quote',
                N'A good migration does one small thing and does it atomically. A bad migration is a 400-line script that renames columns, seeds data, and "also backfills a lookup table - should be fine".',
                NULL, N'Georgia, serif', N'1.05rem', N'italic', NULL
        UNION ALL SELECT 100, N'Heading',
                N'Rename a column without data loss',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 110, N'Text',
                N'EF treats a property rename as drop + add by default, which silently loses the data. Tell it explicitly:',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 120, N'Code',
                N'protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "CoverImage",
        table: "BLOG_POSTS",
        newName: "COVER_IMAGE");
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "COVER_IMAGE",
        table: "BLOG_POSTS",
        newName: "CoverImage");
}',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 130, N'Heading',
                N'Add a NOT NULL column on a table with rows',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 140, N'Text',
                N'SQL Server will reject an ADD COLUMN NOT NULL unless you give it a default. Do it in two migrations if the column should eventually be mandatory with no default:',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 150, N'Code',
                N'// Migration 1 - nullable, backfill, then tighten later
migrationBuilder.AddColumn<string>(
    name: "SLUG",
    table: "BLOG_POSTS",
    nullable: true);

migrationBuilder.Sql(@"
    UPDATE BLOG_POSTS
    SET SLUG = LOWER(REPLACE(TITLE, '' '', ''-''))
    WHERE SLUG IS NULL;
");',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 160, N'Code',
                N'// Migration 2 (next deploy) - now safely NOT NULL
migrationBuilder.AlterColumn<string>(
    name: "SLUG",
    table: "BLOG_POSTS",
    nullable: false,
    defaultValue: "");',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 170, N'Heading',
                N'Apply the script at startup (optional)',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 180, N'Text',
                N'For small apps I run pending migrations on boot. Do not do this in a multi-instance deployment - you will race. Use a job or CI step instead.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 190, N'Code',
                N'using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
    await db.Database.MigrateAsync();
}',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 200, N'Heading',
                N'Takeaway',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 210, N'Text',
                N'Script your migrations, keep each one small and reversible, and treat data fixes as their own commits. Boring migrations are the good ones.',
                NULL, NULL, NULL, NULL, NULL
    )
    INSERT INTO dbo.BLOG_POST_BLOCKS
        (BLOG_POST_ID, BLOCK_TYPE, CONTENT, CODE_LANGUAGE, FONT_FAMILY, FONT_SIZE, FONT_STYLE, TEXT_ALIGN, SORT_ORDER, CREATED_BY, CREATED_DATE)
    SELECT @PostId, type_, content_, lang_, font_, size_, style_, align_, sort_, N'admin', GETDATE()
    FROM blocks;

    PRINT 'Seeded EF Core migrations post id=' + CAST(@PostId AS VARCHAR(20));
END
ELSE
BEGIN
    PRINT 'EF Core migrations post already exists - skipping.';
END
GO
