/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Second demo blog post - Dependency Injection in ASP.NET Core.
          Idempotent: guarded by SLUG.
**/
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.BLOG_POSTS WHERE SLUG = N'dependency-injection-in-aspnet-core')
BEGIN
    DECLARE @PostId INT;

    INSERT INTO dbo.BLOG_POSTS
        (TITLE, SLUG, SUMMARY, TAGS, IS_PUBLISHED, PUBLISHED_DATE, VIEW_COUNT, CREATED_BY, CREATED_DATE)
    VALUES
        (N'Dependency Injection in ASP.NET Core - the practical guide',
         N'dependency-injection-in-aspnet-core',
         N'Scoped vs Transient vs Singleton explained with a real bug each one causes, plus the composition-root pattern I use in every .NET project.',
         N'.NET, DI, Architecture',
         1,
         GETDATE(),
         0,
         N'admin',
         GETDATE());

    SET @PostId = SCOPE_IDENTITY();

    ;WITH blocks(sort_, type_, content_, lang_, font_, size_, style_, align_) AS (
        SELECT  10, N'Heading',
                N'Why DI matters',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 20, N'Text',
                N'Dependency Injection is not about interfaces or "unit testing" - it is about keeping the wiring of your application in exactly one place. When a new controller needs a database, an email sender, and a clock, it asks for them in its constructor. It does not know where they come from. That is the whole trick.',
                NULL, N'Geist, sans-serif', N'1.02rem', NULL, N'justify'
        UNION ALL SELECT 30, N'Heading',
                N'The three lifetimes',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 40, N'Text',
                N'ASP.NET Core ships with three lifetimes. Pick the wrong one and the bugs are subtle.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 50, N'Code',
                N'// one instance per HTTP request - the default for DbContext, services
services.AddScoped<IBlogService, BlogService>();

// a new instance every time someone asks - cheap, stateless helpers
services.AddTransient<IEmailSender, SmtpEmailSender>();

// one instance for the whole process - config, caches, expensive clients
services.AddSingleton<IClock, SystemClock>();',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 60, N'Heading',
                N'The bug each one causes',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 70, N'Text',
                N'* Scoped injected into a Singleton → "Cannot consume scoped service from singleton". ASP.NET catches this at startup if you enable scope validation.\n* Singleton that holds a DbContext → cross-request state leaks, threading errors, "The instance of entity type cannot be tracked" explosions.\n* Transient HttpClient → socket exhaustion. Use IHttpClientFactory instead.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 80, N'Quote',
                N'Rule of thumb: start with Scoped. Move to Singleton only when you have evidence the object is expensive to build AND provably thread-safe. Transient is for pure helpers.',
                NULL, N'Georgia, serif', N'1.05rem', N'italic', NULL
        UNION ALL SELECT 90, N'Heading',
                N'The composition root pattern',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 100, N'Text',
                N'Do not scatter AddScoped calls across Program.cs. Put them in one extension method per layer - the "composition root". Your Web project calls a single AddInfrastructure() and AddApplication() and is done.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 110, N'Code',
                N'// Portfolio.Infrastructure/InfrastructureServiceCollectionExtensions.cs
public static IServiceCollection AddInfrastructure(
    this IServiceCollection services, IConfiguration config)
{
    services.AddDbContext<PortfolioDbContext>(o =>
        o.UseSqlServer(config.GetConnectionString("DefaultConnection")));

    services.AddScoped<IBlogRepo, BlogRepo>();
    services.AddScoped<IProjectRepo, ProjectRepo>();
    // ... every other repo lives here
    return services;
}',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 120, N'Code',
                N'// Program.cs - the whole startup stays short
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 130, N'Heading',
                N'Constructor injection wins',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 140, N'Text',
                N'Prefer constructors over service locators. Your class''s public API makes every dependency visible at the top of the file - no surprise calls to serviceProvider.GetRequiredService() buried three layers deep.',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 150, N'Code',
                N'public class BlogController(IBlogService blog, IWebHostEnvironment env) : BaseController
{
    private readonly IBlogService _blog = blog;
    private readonly IWebHostEnvironment _env = env;

    public async Task<IActionResult> Index() =>
        View(await _blog.GetAllAsync(onlyPublished: true));
}',
                N'csharp', NULL, NULL, NULL, NULL
        UNION ALL SELECT 160, N'Heading',
                N'Takeaway',
                NULL, NULL, NULL, NULL, NULL
        UNION ALL SELECT 170, N'Text',
                N'Use Scoped by default, keep wiring in a composition root, and stick to constructor injection. That is 90% of the DI story in ASP.NET Core.',
                NULL, N'Geist, sans-serif', N'1.02rem', NULL, NULL
    )
    INSERT INTO dbo.BLOG_POST_BLOCKS
        (BLOG_POST_ID, BLOCK_TYPE, CONTENT, CODE_LANGUAGE, FONT_FAMILY, FONT_SIZE, FONT_STYLE, TEXT_ALIGN, SORT_ORDER, CREATED_BY, CREATED_DATE)
    SELECT @PostId, type_, content_, lang_, font_, size_, style_, align_, sort_, N'admin', GETDATE()
    FROM blocks;

    PRINT 'Seeded DI post id=' + CAST(@PostId AS VARCHAR(20));
END
ELSE
BEGIN
    PRINT 'DI post already exists - skipping.';
END
GO
