-- 1. Tạo bảng supplier
USE [laptop_store]
GO

/****** Object:  Table [dbo].[Supplier]    Script Date: 3/25/2024 7:56:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Supplier](
	[Id] [varchar](36) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[ContactName] [nvarchar](255) NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [laptop_store]
CREATE TABLE Customer (
    Id char(36) PRIMARY KEY ,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Email NVARCHAR(255),
    Phone NVARCHAR(20),
    Address NVARCHAR(255)
);

-- 2. Thêm các cột bổ sung
USE [laptop_store]
ALTER TABLE Product
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;
	
	
	USE [laptop_store]
ALTER TABLE Account
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;

USE [laptop_store]
ALTER TABLE Position
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;


USE [laptop_store]
ALTER TABLE ProductCategory
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;


USE [laptop_store]
ALTER TABLE Receipt
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;


USE [laptop_store]
ALTER TABLE ReceiptDetail
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;


USE [laptop_store]
ALTER TABLE WarehouseExport
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;


USE [laptop_store]
ALTER TABLE WarehouseExportDetail
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;

USE [laptop_store]
ALTER TABLE Customer
ADD CreatedDate datetime NULL,
    CreatedBy nvarchar(255) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(255) NULL;


-- 3. Thêm cột image trong Product
USE [laptop_store]
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Product' AND COLUMN_NAME = 'Image')
BEGIN
    ALTER TABLE Product
    ADD Image Text NULL;
END;
