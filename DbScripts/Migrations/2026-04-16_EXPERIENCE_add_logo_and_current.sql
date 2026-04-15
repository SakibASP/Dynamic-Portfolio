/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Add LOGO (file path), IS_CURRENT flag, and COMPANY_URL to
          dbo.EXPERIENCE so the Home page can show the current role
          and a moving marquee of company logos.

          LOGO stores the absolute on-disk path of the uploaded file
          (matching the existing PROFILE_COVER.COVER_IMAGE convention);
          the web layer strips WebRootPath to produce the browser URL.
**/
IF COL_LENGTH('dbo.EXPERIENCE', 'LOGO') IS NULL
    ALTER TABLE dbo.EXPERIENCE ADD LOGO NVARCHAR(512) NULL;
GO
IF COL_LENGTH('dbo.EXPERIENCE', 'IS_CURRENT') IS NULL
    ALTER TABLE dbo.EXPERIENCE ADD IS_CURRENT BIT NOT NULL CONSTRAINT DF_EXPERIENCE_IS_CURRENT DEFAULT(0);
GO
IF COL_LENGTH('dbo.EXPERIENCE', 'COMPANY_URL') IS NULL
    ALTER TABLE dbo.EXPERIENCE ADD COMPANY_URL NVARCHAR(256) NULL;
GO
