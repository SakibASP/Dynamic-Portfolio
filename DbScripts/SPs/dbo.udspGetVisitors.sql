/**
------------------------------------------
Author	: Md. Sakibur Rahman
Date	: 2022-03-01
Optimized: 2025-02-01
Run:
EXEC dbo.udspGetVisitors 
 @PageNumber = 1, @PageSize = 10
,@StartDate = '2024-01-01', @EndDate = '2026-01-01'
,@SearchString = 'Dhaka'
**/
CREATE OR ALTER PROCEDURE dbo.udspGetVisitors
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @SearchString NVARCHAR(128) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Single query approach - eliminates temp table and dynamic SQL
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @SearchPattern NVARCHAR(130) = CASE 
        WHEN @SearchString IS NOT NULL 
        THEN N'%' + UPPER(@SearchString) + N'%' 
        ELSE NULL 
    END;

    ;WITH FilteredVisitors AS (
        SELECT
            V.IPAddress,
            V.OperatingSystem,
            V.Browser,
            V.DeviceBrand,
            V.City,
            V.Country,
            V.VisitTime,
            V.Latitude,
            V.Longitude,
            COUNT(*) OVER() AS TotalRows
        FROM dbo.viewVisitors V WITH(NOLOCK)
        WHERE 
            (@StartDate IS NULL OR @EndDate IS NULL OR TRY_CAST(V.VisitTime AS DATE) BETWEEN @StartDate AND @EndDate)
            AND (@SearchPattern IS NULL OR (
                UPPER(V.DeviceType) LIKE @SearchPattern
                OR UPPER(V.DeviceBrand) LIKE @SearchPattern
                OR UPPER(V.Country) LIKE @SearchPattern
                OR UPPER(V.OperatingSystem) LIKE @SearchPattern
                OR UPPER(V.Browser) LIKE @SearchPattern
                OR UPPER(V.City) LIKE @SearchPattern
            ))
    )
    SELECT 
        IPAddress,
        OperatingSystem,
        Browser,
        DeviceBrand,
        City,
        Country,
        VisitTime,
        Latitude,
        Longitude,
        TotalRows
    FROM FilteredVisitors
    ORDER BY VisitTime DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END