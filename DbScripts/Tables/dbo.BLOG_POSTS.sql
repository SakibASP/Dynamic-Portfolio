/**
------------------------------------------
Author	: Md. Sakibur Rahman
Date	: 2026-04-16
Purpose	: Blog post master table. Each post is a stack of
          ordered content blocks (see dbo.BLOG_POST_BLOCKS).
**/
IF OBJECT_ID('dbo.BLOG_POSTS', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BLOG_POSTS
    (
        AUTO_ID         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        TITLE           NVARCHAR(256)   NOT NULL,
        SLUG            NVARCHAR(256)   NULL,
        SUMMARY         NVARCHAR(1000)  NULL,
        COVER_IMAGE     NVARCHAR(512)   NULL,
        TAGS            NVARCHAR(512)   NULL,
        IS_PUBLISHED    BIT             NOT NULL CONSTRAINT DF_BLOG_POSTS_IS_PUBLISHED DEFAULT(0),
        PUBLISHED_DATE  DATETIME        NULL,
        VIEW_COUNT      INT             NOT NULL CONSTRAINT DF_BLOG_POSTS_VIEW_COUNT DEFAULT(0),
        CREATED_BY      NVARCHAR(256)   NULL,
        CREATED_DATE    DATETIME        NOT NULL CONSTRAINT DF_BLOG_POSTS_CREATED_DATE DEFAULT(GETDATE()),
        UPDATED_BY      NVARCHAR(256)   NULL,
        UPDATED_DATE    DATETIME        NULL
    );

    CREATE INDEX IX_BLOG_POSTS_Published ON dbo.BLOG_POSTS (IS_PUBLISHED, PUBLISHED_DATE DESC);
    CREATE UNIQUE INDEX UX_BLOG_POSTS_Slug ON dbo.BLOG_POSTS (SLUG) WHERE SLUG IS NOT NULL;
END
GO
