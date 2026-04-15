/**
------------------------------------------
Author  : Md. Sakibur Rahman
Date    : 2026-04-16
Purpose : Drive the Home hero's "Status" and "Stack" meta chips from
          MY_PROFILE instead of hard-coding them.
**/
IF COL_LENGTH('dbo.MY_PROFILE', 'STATUS') IS NULL
    ALTER TABLE dbo.MY_PROFILE ADD STATUS NVARCHAR(64) NULL;
GO
IF COL_LENGTH('dbo.MY_PROFILE', 'TECH_STACK') IS NULL
    ALTER TABLE dbo.MY_PROFILE ADD TECH_STACK NVARCHAR(256) NULL;
GO

UPDATE dbo.MY_PROFILE
   SET STATUS     = COALESCE(STATUS,     N'Open to work'),
       TECH_STACK = COALESCE(TECH_STACK, N'.NET · SQL · Clean Arch');
GO
