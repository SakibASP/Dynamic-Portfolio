/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Switch all new image columns from VARBINARY to NVARCHAR
          (absolute server path on disk; the web layer strips wwwroot
          to build the browser URL, matching the existing PROFILE_COVER
          convention).
**/

-- BLOG_POSTS.COVER_IMAGE : varbinary -> nvarchar(512)
IF EXISTS (
    SELECT 1 FROM sys.columns c
    JOIN sys.types t ON t.user_type_id = c.user_type_id
    WHERE c.object_id = OBJECT_ID('dbo.BLOG_POSTS')
      AND c.name = 'COVER_IMAGE' AND t.name = 'varbinary'
)
BEGIN
    ALTER TABLE dbo.BLOG_POSTS DROP COLUMN COVER_IMAGE;
    ALTER TABLE dbo.BLOG_POSTS ADD COVER_IMAGE NVARCHAR(512) NULL;
END
GO

-- BLOG_POST_BLOCKS.IMAGE_DATA (varbinary) -> IMAGE_PATH (nvarchar)
IF COL_LENGTH('dbo.BLOG_POST_BLOCKS', 'IMAGE_DATA') IS NOT NULL
    ALTER TABLE dbo.BLOG_POST_BLOCKS DROP COLUMN IMAGE_DATA;
GO
IF COL_LENGTH('dbo.BLOG_POST_BLOCKS', 'IMAGE_PATH') IS NULL
    ALTER TABLE dbo.BLOG_POST_BLOCKS ADD IMAGE_PATH NVARCHAR(512) NULL;
GO

-- EXPERIENCE.LOGO : varbinary -> nvarchar(512)
IF EXISTS (
    SELECT 1 FROM sys.columns c
    JOIN sys.types t ON t.user_type_id = c.user_type_id
    WHERE c.object_id = OBJECT_ID('dbo.EXPERIENCE')
      AND c.name = 'LOGO' AND t.name = 'varbinary'
)
BEGIN
    ALTER TABLE dbo.EXPERIENCE DROP COLUMN LOGO;
    ALTER TABLE dbo.EXPERIENCE ADD LOGO NVARCHAR(512) NULL;
END
GO
