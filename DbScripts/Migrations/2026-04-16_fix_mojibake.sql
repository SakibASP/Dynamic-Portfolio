/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Scrub Windows-1252 mojibake left over from seed inserts where
          em/en-dashes, smart quotes and bullets got stored as their
          mis-decoded UTF-8 byte sequences.
**/
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- em dash —
UPDATE dbo.BLOG_POST_BLOCKS SET CONTENT = REPLACE(CONTENT, N'â€”', N'—') WHERE CONTENT LIKE N'%â€”%';
UPDATE dbo.BLOG_POSTS       SET TITLE   = REPLACE(TITLE,   N'â€”', N'—') WHERE TITLE   LIKE N'%â€”%';
UPDATE dbo.BLOG_POSTS       SET SUMMARY = REPLACE(SUMMARY, N'â€”', N'—') WHERE SUMMARY LIKE N'%â€”%';

-- en dash –
UPDATE dbo.BLOG_POST_BLOCKS SET CONTENT = REPLACE(CONTENT, N'â€“', N'–') WHERE CONTENT LIKE N'%â€“%';
UPDATE dbo.BLOG_POSTS       SET TITLE   = REPLACE(TITLE,   N'â€“', N'–') WHERE TITLE   LIKE N'%â€“%';
UPDATE dbo.BLOG_POSTS       SET SUMMARY = REPLACE(SUMMARY, N'â€“', N'–') WHERE SUMMARY LIKE N'%â€“%';

-- curly single quotes ‘ ’
UPDATE dbo.BLOG_POST_BLOCKS SET CONTENT = REPLACE(CONTENT, N'â€˜', N'‘') WHERE CONTENT LIKE N'%â€˜%';
UPDATE dbo.BLOG_POST_BLOCKS SET CONTENT = REPLACE(CONTENT, N'â€™', N'’') WHERE CONTENT LIKE N'%â€™%';

-- curly double quotes “ ”
UPDATE dbo.BLOG_POST_BLOCKS SET CONTENT = REPLACE(CONTENT, N'â€œ', N'“') WHERE CONTENT LIKE N'%â€œ%';
UPDATE dbo.BLOG_POST_BLOCKS SET CONTENT = REPLACE(CONTENT, N'â€', N'”')  WHERE CONTENT LIKE N'%â€%';

-- bullet •
UPDATE dbo.BLOG_POST_BLOCKS SET CONTENT = REPLACE(CONTENT, N'â€¢', N'•') WHERE CONTENT LIKE N'%â€¢%';

-- middot ·
UPDATE dbo.BLOG_POST_BLOCKS SET CONTENT = REPLACE(CONTENT, N'Â·',  N'·') WHERE CONTENT LIKE N'%Â·%';
GO

SELECT TOP 10 AUTO_ID, LEFT(CONTENT, 100) AS Preview
FROM dbo.BLOG_POST_BLOCKS
WHERE CONTENT LIKE N'%â%' OR CONTENT LIKE N'%€%' OR CONTENT LIKE N'%Â%';
GO
