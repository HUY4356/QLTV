use [ThuVienDB];
go

INSERT INTO MuonTra (the_id, DanhMucSach_id, DueDate, TrangThai)
VALUES (1, 1, '2025-09-01', 'open');

UPDATE MuonTra
SET NgayTra = '2025-09-07'
WHERE id = 1;

SELECT * FROM MuonTra WHERE id = 1;
