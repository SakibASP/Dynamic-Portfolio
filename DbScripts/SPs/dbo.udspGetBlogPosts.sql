/**
------------------------------------------
Author	: Md. Sakibur Rahman
Date	: 2026-04-16
Run:
EXEC dbo.udspGetBlogPosts
     @PageNumber = 1, @PageSize = 10
   , @SearchString = N'auth'
   , @OnlyPublished = 1
**/
CREATE OR ALTER PROCEDURE dbo.udspGetBlogPosts
    @PageNumber     INT = 1,
    @PageSize       INT = 10,
    @SearchString   NVARCHAR(256) = NULL,
    @OnlyPublished  BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @Pattern NVARCHAR(260) = CASE
        WHEN @SearchString IS NOT NULL
        THEN N'%' + UPPER(@SearchString) + N'%'
        ELSE NULL
    END;

    ;WITH Filtered AS (
        SELECT
            P.AUTO_ID,
            P.TITLE,
            P.SLUG,
            P.SUMMARY,
            P.COVER_IMAGE,
            P.TAGS,
            P.IS_PUBLISHED,
            P.PUBLISHED_DATE,
            P.VIEW_COUNT,
            P.CREATED_BY,
            P.CREATED_DATE,
            P.UPDATED_BY,
            P.UPDATED_DATE,
            COUNT(*) OVER() AS TotalRows
        FROM dbo.BLOG_POSTS P WITH(NOLOCK)
        WHERE (@OnlyPublished = 0 OR P.IS_PUBLISHED = 1)
          AND (@Pattern IS NULL OR UPPER(P.TITLE)   LIKE @Pattern
                                OR UPPER(P.SUMMARY) LIKE @Pattern
                                OR UPPER(P.TAGS)    LIKE @Pattern)
    )
    SELECT *
    FROM Filtered
    ORDER BY COALESCE(PUBLISHED_DATE, CREATED_DATE) DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
