/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Insert a demo blog post + mixed blocks so the Blog UI has
          something to render. Safe to run multiple times: guarded
          by a SLUG check.
**/
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.BLOG_POSTS WHERE SLUG = N'setup-authentication-in-dotnet')
BEGIN
    DECLARE @PostId INT;

    INSERT INTO dbo.BLOG_POSTS
        (TITLE, SLUG, SUMMARY, TAGS, IS_PUBLISHED, PUBLISHED_DATE, VIEW_COUNT, CREATED_BY, CREATED_DATE)
    VALUES
        (N'Setting up authentication in .NET 10',
         N'setup-authentication-in-dotnet',
         N'A quick walkthrough of wiring ASP.NET Core Identity with cookie auth, protecting controllers, and adding a login page — the way I do it in every new project.',
         N'.NET, Auth, Tutorial',
         1,
         GETDATE(),
         0,
         N'admin',
         GETDATE());

    SET @PostId = SCOPE_IDENTITY();

    ;WITH blocks(sort_, type_, content_, lang_, font_, size_, style_, align_) AS (
        SELECT  10, N'Heading',
                N'Why we need authentication',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 20, N'Text',
                N'Most applications eventually need to know who the user is. ASP.NET Core ships with a full Identity stack that covers password hashing, cookies, account confirmation, and 2FA. In this post I will show the minimum wiring that gets you from a blank Web project to a working login page.',
                NULL, N'Geist, sans-serif', N'1.02rem', NULL, N'justify'
        UNION ALL SELECT 30, N'Heading',
                N'1. Install the packages',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 40, N'Text',
                N'Add the Identity + EF Core packages to your Web project. If you scaffolded the app with the "Individual Accounts" template they are already there.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 50, N'Code',
                N'dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer',
                N'bash', NULL, NULL, NULL, NULL
        UNION ALL SELECT 60, N'Heading',
                N'2. Extend your DbContext',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 70, N'Text',
                N'Inherit from IdentityDbContext<IdentityUser> so Identity can store users/roles/claims alongside your domain tables. Same database, same migrations.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 80, N'Code',
                N'public class PortfolioDbContext : IdentityDbContext<IdentityUser>
{
    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> opts) : base(opts) { }

    public DbSet<PROJECTS> PROJECTS { get; set; } = default!;
    public DbSet<BLOG_POSTS> BLOG_POSTS { get; set; } = default!;
}',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 90, N'Heading',
                N'3. Register in Program.cs',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 100, N'Code',
                N'services.AddDbContext<PortfolioDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PortfolioDbContext>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();  // /Identity/Account/Login, /Register, etc.',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 110, N'Heading',
                N'4. Protect a controller',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 120, N'Text',
                N'Use the [Authorize] attribute to gate an action. Unauthenticated users will be redirected to /Identity/Account/Login automatically.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 130, N'Code',
                N'[Authorize]
public class BlogController : Controller
{
    public IActionResult Manage() => View();
}',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 140, N'Quote',
                N'Tip: keep public read actions open and put [Authorize] only on the write/admin endpoints. Easier to reason about than filtering by URL.',
                NULL, N'Georgia, serif', N'1.05rem', N'italic', NULL
        UNION ALL SELECT 150, N'Heading',
                N'5. Run it',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 160, N'Code',
                N'dotnet ef migrations add AddIdentity
dotnet ef database update
dotnet run',
                N'bash', NULL, NULL, NULL, NULL
        UNION ALL SELECT 170, N'Text',
                N'That is it. Browse to /Identity/Account/Register, create a user, sign in, and hit any [Authorize]-gated URL — you are in.',
                NULL, NULL, NULL, NULL, NULL
    )
    INSERT INTO dbo.BLOG_POST_BLOCKS
        (BLOG_POST_ID, BLOCK_TYPE, CONTENT, CODE_LANGUAGE, FONT_FAMILY, FONT_SIZE, FONT_STYLE, TEXT_ALIGN, SORT_ORDER, CREATED_BY, CREATED_DATE)
    SELECT @PostId, type_, content_, lang_, font_, size_, style_, align_, sort_, N'admin', GETDATE()
    FROM blocks;

    PRINT 'Seeded demo blog post id=' + CAST(@PostId AS VARCHAR(20));
END
ELSE
BEGIN
    PRINT 'Demo post already exists — skipping.';
END
GO
