/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Insert Dependable Solutions Inc as the new current role,
          add a few description bullets, and re-shuffle SORT_ORDER so
          the home-page marquee / current-role pill come out as:
            1. Dependable Solutions   (current, 40)
            2. BRAC Saajan             (30)
            3. CSS Bangladesh          (20)
            4. Khan IT                 (10)
            5. Mobarak/M.K/Jhenaidah   ( 0, no logo)
**/
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- 1) Un-flag previous current, re-seat SORT_ORDER
UPDATE dbo.EXPERIENCE SET IS_CURRENT = 0;
UPDATE dbo.EXPERIENCE SET SORT_ORDER = 30 WHERE INSTITUTE = N'Brac Saajan';
UPDATE dbo.EXPERIENCE SET SORT_ORDER = 20 WHERE INSTITUTE = N'CSS Bangladesh';
UPDATE dbo.EXPERIENCE SET SORT_ORDER = 10 WHERE INSTITUTE = N'Khan IT';
UPDATE dbo.EXPERIENCE SET SORT_ORDER = 0  WHERE INSTITUTE LIKE N'Mobarak%';

-- 2) Insert Dependable Solutions (guarded — safe to re-run)
IF NOT EXISTS (SELECT 1 FROM dbo.EXPERIENCE WHERE INSTITUTE = N'Dependable Solutions Inc')
BEGIN
    INSERT INTO dbo.EXPERIENCE
        (DESIGNATION, FROM_DATE, TO_DATE, INSTITUTE, SORT_ORDER, LOGO, IS_CURRENT, COMPANY_URL)
    VALUES
        (N'Software Engineer',
         N'2026',
         N'Present',
         N'Dependable Solutions Inc',
         40,
         N'F:\Cloud Coding\GitHub\SakibASP\Dynamic-Portfolio\Portfolio.Web\wwwroot\Images\Experience\dependable-solutions.png',
         1,
         N'https://www.dependablesolutions.com/');

    DECLARE @ExpId INT = SCOPE_IDENTITY();

    ;WITH bullets(sort_, txt_) AS (
        SELECT 1, N'Brand licensing, royalty and approval platform — Dependable Rights Manager (DRM)'
        UNION ALL SELECT 2, N'C#, ASP.NET Core, MS SQL Server, EF Core'
        UNION ALL SELECT 3, N'Feature development, code review and bug triage across the licensing, approval and royalty modules'
        UNION ALL SELECT 4, N'Collaboration with the Los Angeles HQ and the Bangladesh QA team on release readiness'
    )
    INSERT INTO dbo.DESCRIPTION
        (CREATED_BY, CREATED_DATE, DESCRIPTION_TEXT, TYPE_ID, SORT_ORDER, EXPERIENCE_ID)
    SELECT N'sakibur.rahman.cse@gmail.com', GETDATE(), txt_, 1, sort_, @ExpId
    FROM bullets;

    PRINT 'Inserted Dependable Solutions Inc experience_id=' + CAST(@ExpId AS VARCHAR(20));
END
ELSE
BEGIN
    UPDATE dbo.EXPERIENCE
       SET IS_CURRENT = 1, SORT_ORDER = 40
     WHERE INSTITUTE = N'Dependable Solutions Inc';
    PRINT 'Dependable Solutions already exists — marked current and re-sorted.';
END
GO

SELECT AUTO_ID, INSTITUTE, DESIGNATION, FROM_DATE, TO_DATE, IS_CURRENT, SORT_ORDER
FROM dbo.EXPERIENCE
ORDER BY SORT_ORDER DESC;
GO
