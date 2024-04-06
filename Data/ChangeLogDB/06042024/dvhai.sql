
-- Thêm mã code cho 2 phân hệ
USE [laptop_store];

ALTER TABLE dbo.Receipt
ADD Code VARCHAR(20) DEFAULT NULL;

ALTER TABLE dbo.WarehouseExport
ADD Code VARCHAR(20) DEFAULT NULL;