﻿CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[FirstName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[Email] VARCHAR(100) NOT NULL,
	[Doctor] VARCHAR(50) NOT NULL,
	[AddedOn] DATE NOT NULL DEFAULT GETDATE(),
	[PickupDate] DATE NOT NULL 
)
