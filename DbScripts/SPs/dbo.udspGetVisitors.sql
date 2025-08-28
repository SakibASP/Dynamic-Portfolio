
/**

EXEC dbo.udspGetVisitors 
@PageNumber = 1,@PageSize=10
,@StartDate='2024-01-01',@EndDate = '2026-01-01'
,@SearchString = 'Dhaka'

**/

CREATE OR ALTER PROCEDURE [dbo].[udspGetVisitors]
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @SearchString NVARCHAR(128) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH FilteredVisitors AS (
        SELECT 
            *,
            TotalRows = COUNT(*) OVER ()
        FROM dbo.Visitors V
        WHERE 
            (@StartDate IS NULL OR @EndDate IS NULL OR 
                TRY_CONVERT(DATE, V.VisitTime) BETWEEN TRY_CONVERT(DATE, @StartDate) AND TRY_CONVERT(DATE, @EndDate))
            AND (
                @SearchString IS NULL OR @SearchString = '' OR 
                UPPER(V.DeviceType) LIKE '%' + UPPER(@SearchString) + '%' OR
                UPPER(V.DeviceBrand) LIKE '%' + UPPER(@SearchString) + '%' OR
                UPPER(V.Country) LIKE '%' + UPPER(@SearchString) + '%' OR
                UPPER(V.OperatingSystem) LIKE '%' + UPPER(@SearchString) + '%' OR
                UPPER(V.Browser) LIKE '%' + UPPER(@SearchString) + '%' OR
                UPPER(V.City) LIKE '%' + UPPER(@SearchString) + '%'
            )
    )
    SELECT 
        IPAddress,
        OperatingSystem,
        OperatingSystemVersion,
        Browser,
        BrowserVersion,
        DeviceType,
        DeviceBrand,
        DeviceModel,
        City,
        Country,
        Timezone,
        VisitTime,
        TotalRows
    FROM FilteredVisitors
    ORDER BY VisitTime DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

END