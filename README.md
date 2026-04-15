# Dynamic Portfolio

A personal portfolio + blog platform built on **ASP.NET Core 10** with **Clean Architecture**, **EF Core (DB-first)** and **SQL Server**. It hosts my About / Resume / Projects pages, a visitor-tracking pipeline, and a full blog posting system with block-based content (text, code, images, quotes).

Live layout is dark-themed with Geist typography, AOS scroll animations and a magnetic-button hero.

---

## Features

### Public
- **Home** — hero with typed role rotator, current-company pill, moving company-logo marquee, featured blog posts
- **About / Resume / Projects / Contact** — all DB-driven
- **Blog** — published posts list, full reader view with syntax-highlighted code blocks and a copy-to-clipboard button
- **Visitor tracking** — IP / geo / OS / browser / device logged via a global action filter

### Admin (cookie-auth, ASP.NET Core Identity)
- CRUD for profile, cover, skills, education, experience, projects, descriptions, CV
- **Blog admin** — create posts, add/edit/reorder/delete content blocks, toggle publish state
- Supported block types: **Heading, Text, Code, Image, Quote** — each with optional font family / size / style / alignment overrides
- Unread contact-message badge in the admin menu

---

## Architecture

```
Portfolio.Domain            ← entities (no dependencies)
Portfolio.Application       ← use-case services, abstractions, DTOs, common helpers
Portfolio.Infrastructure    ← EF Core DbContext, repository impls, external adapters
Portfolio.Web               ← ASP.NET Core MVC host (controllers + Razor views)
Portfolio.Utils             ← utilities
DbScripts/                  ← Tables, SPs, Views, Migrations, Seeds
```

- **Repository** + **Service** pattern, registered via `AddInfrastructure()` / `AddApplication()` composition roots
- **DB-first**: entities carry `[Table]` / `[Key]` attributes; schema is managed by scripts under `DbScripts/`
- Connection string is **encrypted at rest** in `appsettings.json` and decrypted at startup via `EncryptionHelper`
- **Image storage convention**: files are saved under `wwwroot/Images/<section>/` and the absolute server path is stored in the DB; views strip `WebRootPath` to build the browser URL

---

## Project layout

| Folder | Purpose |
| --- | --- |
| `Portfolio.Domain/` | Entities: `MY_PROFILE`, `PROJECTS`, `EXPERIENCE`, `EDUCATION`, `MY_SKILLS`, `CONTACTS`, `BLOG_POSTS`, `BLOG_POST_BLOCKS`, etc. |
| `Portfolio.Application/Abstractions/Persistence/` | `IxxxRepo` contracts |
| `Portfolio.Application/Services/` | `IxxxService` + impls |
| `Portfolio.Application/Common/` | `Constant`, `AppClock`, `GenerateParameter`, `SaveRequestModel`, `EncryptionHelper`, `EmailSettings` (last two are git-ignored) |
| `Portfolio.Infrastructure/Data/` | `PortfolioDbContext` |
| `Portfolio.Infrastructure/Adapters/` | Email / geo-location / user-agent / visitor-tracking adapters |
| `Portfolio.Web/Controllers/` | MVC controllers |
| `Portfolio.Web/Views/` | Razor views (Home, Blog, MY_PROFILE, PROJECTS, EXPERIENCE, …) |
| `Portfolio.Web/wwwroot/` | Static assets, dark theme, blog CSS, uploads under `Images/` |
| `DbScripts/Tables/` | `CREATE TABLE` scripts |
| `DbScripts/Views/` | SQL views |
| `DbScripts/SPs/` | Stored procedures (prefixed `udsp`) |
| `DbScripts/Migrations/` | Additive schema changes |
| `DbScripts/Seeds/` | Demo data inserts |

---

## Getting started

### Prerequisites
- **.NET 10 SDK**
- **SQL Server** (local or remote)
- Visual Studio 2022 / VS Code / Rider

### 1. Clone

```bash
git clone https://github.com/SakibASP/Dynamic-Portfolio.git
cd Dynamic-Portfolio
```

### 2. Create the database

```bash
# Any empty SQL Server database will work, e.g.:
sqlcmd -S localhost -U sa -P 'YourPassword' -Q "CREATE DATABASE db_sakib_portfolio"
```

Run every file under `DbScripts/Tables/` first, then `DbScripts/Views/`, `DbScripts/SPs/`, any `DbScripts/Migrations/` in date order, and finally (optional) `DbScripts/Seeds/` for sample data:

```bash
sqlcmd -S localhost -U sa -P 'YourPassword' -C -I -d db_sakib_portfolio \
    -i DbScripts/Tables/dbo.BLOG_POSTS.sql \
    -i DbScripts/Tables/dbo.BLOG_POST_BLOCKS.sql \
    -i DbScripts/SPs/dbo.udspGetBlogPosts.sql \
    -i DbScripts/Migrations/2026-04-16_EXPERIENCE_add_logo_and_current.sql \
    -i DbScripts/Seeds/demo_blog_post.sql
```

The existing ASP.NET Core Identity tables (`AspNetUsers`, etc.) are created by EF migrations when the app first runs.

### 3. Supply secrets

Two files are git-ignored and must exist locally before the app can build or run:

**`Portfolio.Application/Common/EncryptionHelper.cs`**

```csharp
namespace Portfolio.Application.Common;

public static class EncryptionHelper
{
    public static string Encrypt(string plain) { /* your AES/ProtectedData impl */ }
    public static string Decrypt(string cipher) { /* your AES/ProtectedData impl */ }
}
```

**`Portfolio.Application/Common/EmailSettings.cs`**

```csharp
namespace Portfolio.Application.Common;

public class EmailSettings
{
    public string SmtpServer { get; set; } = "";
    public int    Port       { get; set; }
    public string UserName   { get; set; } = "";
    public string Password   { get; set; } = "";
    public string From       { get; set; } = "";
}
```

Update `appsettings.json` with your encrypted connection string (use `EncryptionHelper.Encrypt`).

### 4. Run

```bash
dotnet restore
dotnet run --project Portfolio.Web
```

Register an admin account at `/Identity/Account/Register`, then access the admin menu in the navbar.

---

## Blog authoring

1. Admin → **Manage Blog** → **New post**
2. After saving, you land on the post's **Edit** page where you can stack content blocks:
   - **Heading** — section title
   - **Text** — prose with optional font family / size / bold-italic / alignment
   - **Code** — paste the raw snippet and pick the language (`csharp`, `javascript`, `sql`, `bash`, …); readers get syntax highlighting via highlight.js and a copy button
   - **Image** — uploaded to `wwwroot/Images/Blog/Blocks/` with an optional caption
   - **Quote** — styled callout
3. Re-order blocks with ↑ / ↓, toggle **Published** when ready

View counts auto-increment on read; drafts are visible only to authenticated admins.

---

## Database conventions

- Stored procedures are prefixed **`udsp`** (user-defined): `udspGetVisitors`, `udspGetBlogPosts`, …
- SQL views are prefixed **`view`**: `viewVisitors`
- Timestamps are stored in **Bangladesh Standard Time** (`Asia/Dhaka`) via `AppClock.BdNow`
- Audit columns on most tables: `CREATED_BY`, `CREATED_DATE`, `UPDATED_BY`, `UPDATED_DATE`

---

## Tech stack

- ASP.NET Core 10 MVC + Razor Pages (Identity)
- EF Core 10 (SQL Server)
- Serilog
- Bootstrap 5, AOS, GLightbox, Swiper, highlight.js (via CDN)
- Geist + Geist Mono (Google Fonts)

---

## License

Personal portfolio — feel free to use the structure as a reference for your own projects. Content (text, images, CV) belongs to the author.

— **[SakibASP](https://github.com/SakibASP)**
