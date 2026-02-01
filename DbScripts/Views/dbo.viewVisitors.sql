
/**
------------------------------------------
Author	: Md. Sakibur Rahman
Date	: 2022-03-01

Run:

SELECT * FROM dbo.viewVisitors

**/

CREATE OR ALTER VIEW dbo.viewVisitors

AS

SELECT DISTINCT
	IPAddress,
	OperatingSystem,
	Browser,
	DeviceBrand,
	DeviceType,
	City,
	Country,
	VisitTime,
	Latitude,
	Longitude
FROM dbo.Visitors V WITH(NOLOCK)

UNION ALL

SELECT
	IPAddress,
	OperatingSystem,
	Browser,
	DeviceBrand,
	DeviceType,
	City,
	Country,
	VisitTime,
	NULL Latitude,
	NULL Longitude
FROM dbo.VisitorsArchive V WITH(NOLOCK)

