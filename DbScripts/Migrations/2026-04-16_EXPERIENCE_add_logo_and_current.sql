/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Add LOGO (image bytes) + IS_CURRENT flag + COMPANY_URL
          to dbo.EXPERIENCE so Home page can show current role and
          a moving marquee of company logos.
**/
IF COL_LENGTH('dbo.EXPERIENCE', 'LOGO') IS NULL
    ALTER TABLE dbo.EXPERIENCE ADD LOGO VARBINARY(MAX) NULL;
GO
IF COL_LENGTH('dbo.EXPERIENCE', 'IS_CURRENT') IS NULL
    ALTER TABLE dbo.EXPERIENCE ADD IS_CURRENT BIT NOT NULL CONSTRAINT DF_EXPERIENCE_IS_CURRENT DEFAULT(0);
GO
IF COL_LENGTH('dbo.EXPERIENCE', 'COMPANY_URL') IS NULL
    ALTER TABLE dbo.EXPERIENCE ADD COMPANY_URL NVARCHAR(256) NULL;
GO
