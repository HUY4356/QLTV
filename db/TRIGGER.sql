use ThuVienDB;
GO 
--Tự động tính phạt khi trả muộn:
CREATE OR ALTER TRIGGER TRG_MuonTra_TinhPhat_OnReturn
ON dbo.MuonTra
AFTER UPDATE
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE mt
  SET TienPhat = CASE 
      WHEN inserted.NgayTra IS NOT NULL AND inserted.NgayTra > inserted.DueDate
      THEN DATEDIFF(DAY, inserted.DueDate, inserted.NgayTra) * ISNULL(ts.MucPhatNgay, 5000)
      ELSE 0
  END,
      TrangThai = CASE 
         WHEN inserted.NgayTra IS NOT NULL THEN 'returned'
         ELSE mt.TrangThai
      END
  FROM MuonTra mt
  JOIN inserted ON mt.id = inserted.id
  CROSS APPLY (SELECT CAST(5000 AS DECIMAL(10,2)) AS MucPhatNgay) ts; -- hoặc SELECT từ bảng ThamSoHeThong
END;

go
CREATE OR ALTER FUNCTION dbo.fn_Overlap(@PhongId INT, @Start DATETIME, @End DATETIME)
RETURNS BIT
AS
BEGIN
  RETURN (
    SELECT CASE WHEN EXISTS (
      SELECT 1
      FROM DatPhong
      WHERE Phong_id = @PhongId
        AND TrangThai IN ('pending','approved','checkedin')
        AND NOT (@End <= GioBatDau OR @Start >= GioKetThuc)
    ) THEN 1 ELSE 0 END
  );
END;

--Chặn chồng lấn lịch phòng:
GO
CREATE OR ALTER PROCEDURE sp_DatPhong_Tao
  @UserId INT, @PhongId INT, @Start DATETIME, @End DATETIME
AS
BEGIN
  IF dbo.fn_Overlap(@PhongId, @Start, @End) = 1
    BEGIN RAISERROR('Khung giờ đã có người đặt', 16, 1); RETURN; END

  INSERT INTO DatPhong(user_id, Phong_id, NgayDat, GioBatDau, GioKetThuc, TrangThai)
  VALUES (@UserId, @PhongId, GETDATE(), @Start, @End, 'approved');
END;

--Đánh dấu no-show & xử lý vi phạm:
go
CREATE OR ALTER PROCEDURE sp_DatPhong_NoShow_IfLate
AS
BEGIN
  UPDATE DatPhong
  SET TrangThai = 'noshow'
  WHERE TrangThai = 'approved'
    AND DATEADD(MINUTE, 15, GioBatDau) < GETDATE(); 

END;

--Top 10 sách mượn nhiều:
go
SELECT dms.id, dms.TenSach, COUNT(*) AS LuotMuon
FROM MuonTra mt
JOIN DanhMucSach dms ON dms.id = mt.DanhMucSach_id
GROUP BY dms.id, dms.TenSach
ORDER BY LuotMuon DESC
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

--Sách “ngủ đông” (6 tháng không ai mượn):
go
SELECT dms.id, dms.TenSach
FROM DanhMucSach dms
LEFT JOIN MuonTra mt ON mt.DanhMucSach_id = dms.id AND mt.NgayMuon >= DATEADD(MONTH,-6,GETDATE())
WHERE mt.id IS NULL;

--Tỉ lệ trễ hạn theo tháng:
go
SELECT FORMAT(NgayMuon,'yyyy-MM') AS Thang,
       SUM(CASE WHEN NgayTra > DueDate THEN 1 ELSE 0 END)*1.0 / COUNT(*) AS TyLeTreHan
FROM MuonTra
GROUP BY FORMAT(NgayMuon,'yyyy-MM')
ORDER BY Thang;
