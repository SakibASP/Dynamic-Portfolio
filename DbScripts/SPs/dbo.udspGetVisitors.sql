
/**

EXEC dbo.udspGetVisitors 
@PageNumber = 4,@PageSize=10
,@StartDate='2024-01-01',@EndDate = '2025-01-01'
,@SearchString = 'Dhaka'

**/

CREATE OR ALTER PROCEDURE dbo.udspGetVisitors
    @PageNumber INT = 1,
    @PageSize INT = 10,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@SearchString NVARCHAR(128)=NULL
AS

BEGIN

DROP TABLE IF EXISTS #tmpVisitor;
CREATE TABLE #tmpVisitor(
	IPAddress [nvarchar](256) NULL,
	OperatingSystem [nvarchar](256) NULL,
	OperatingSystemVersion [nvarchar](256) NULL,
	Browser [nvarchar](271) NULL,
	BrowserVersion [nvarchar](256) NULL,
	DeviceType [nvarchar](100) NULL,
	DeviceBrand [nvarchar](256) NULL,
	DeviceModel [nvarchar](256) NULL,
	City [nvarchar](256) NULL,
	Country [nvarchar](256) NULL,
	Timezone [nvarchar](256) NULL,
	VisitTime [Datetime] NULL
)

DECLARE @sql NVARCHAR(MAX) = '';
IF (@StartDate IS NOT NULL AND @EndDate IS NOT NULL)
	SET @sql = @sql+' (TRY_CAST(V.VisitTime AS DATE) BETWEEN '''+TRY_CAST(@StartDate AS nvarchar(256))+''' AND '''+TRY_CAST(@EndDate AS nvarchar(256))+''') AND';
IF ISNULL(@SearchString,'') <> ''
	SET @sql = @sql+' (UPPER(V.DeviceType) LIKE N''%'+UPPER(@SearchString)+'%'' OR UPPER(V.DeviceBrand) LIKE N''%'+UPPER(@SearchString)+'%'' OR UPPER(V.Country) LIKE N''%'+UPPER(@SearchString)+'%''
					  OR UPPER(V.OperatingSystem) LIKE N''%'+UPPER(@SearchString)+'%'' OR UPPER(V.Browser) LIKE N''%'+UPPER(@SearchString)+'%'' OR UPPER(V.City) LIKE N''%'+UPPER(@SearchString)+'%'')
					  AND';

--Removing last 'AND'
IF ISNULL(@sql,'') <> ''
	SET @sql= (LEFT(@sql, LEN(@sql) -3));

DECLARE @query AS NVARCHAR(MAX);
SET @query=N'
--FROM BRANCH AUDIT TABLE
INSERT INTO #tmpVisitor(	
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
	VisitTime
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
	VisitTime
FROM dbo.Visitors V WITH(NOLOCK)
'+CASE WHEN @sql!='' THEN ' WHERE '+ @sql ELSE '' END

PRINT @query;
exec sp_executesql @query,N'@PageNumber INT=1, @PageSize INT=10,@StartDate DATETIME = NULL,@EndDate DATETIME = NULL,@SearchString NVARCHAR(128)=NULL',
@PageNumber=@PageNumber,@PageSize=@PageSize,@StartDate=@StartDate,@EndDate=@EndDate,@SearchString=@SearchString;

DECLARE @RecordCount INT = 0;
SET @RecordCount = (SELECT COUNT(*) FROM #tmpVisitor); 

SELECT *,@RecordCount AS TotalRows FROM #tmpVisitor A
ORDER BY A.VisitTime DESC
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;

DROP TABLE IF EXISTS #tmpVisitor;

END