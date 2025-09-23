USE [ThuVienDB];
go 

-- =============================
-- ROLE & HẠN MỨC
-- =============================
INSERT INTO Role (name) VALUES (N'reader'), (N'librarian'), (N'admin');

INSERT INTO HanMuc (role_id, MaxBooks, MaxDays, MaxRenewals, MaxFines)
VALUES 
(1, 5, 14, 2, 100000),   -- reader: 5 sách, 14 ngày, 2 lần gia hạn, phạt tối đa 100k
(2, 10, 30, 3, 0),       -- librarian: hạn mức lớn hơn
(3, 20, 60, 5, 0);       -- admin: gần như không giới hạn

-- =============================
-- USER
-- =============================
INSERT INTO [User] (fullname, email, phone, password, role_id)
VALUES
(N'Nguyễn Văn A', 'a@example.com', '0901234567', '123456', 1), -- reader
(N'Trần Thị B', 'b@example.com', '0912345678', '123456', 2),   -- librarian
(N'Lê Văn C', 'c@example.com', '0923456789', '123456', 3);     -- admin

-- =============================
-- THẺ BẠN ĐỌC
-- =============================
INSERT INTO The (The_code, user_id, HanThe, TrangThai)
VALUES
(N'TH001', 1, DATEADD(YEAR,1,GETDATE()), 'active'),
(N'TH002', 2, DATEADD(YEAR,1,GETDATE()), 'active');

-- =============================
-- NHÓM SÁCH & DANH MỤC SÁCH
-- =============================
INSERT INTO NhomSach (MaNhom, TenNhom)
VALUES (N'CN', N'Công nghệ'), (N'VH', N'Văn học');

INSERT INTO DanhMucSach (NhomSach_id, MaSach, TenSach, TacGia, DonGia, SLTon, ViTriKe)
VALUES
(1, N'CN001', N'Lập trình C++ cơ bản', N'Nguyễn Văn D', 85000, 10, N'Kệ A1'),
(1, N'CN002', N'Cấu trúc dữ liệu và Giải thuật', N'Lê Thị E', 99000, 5, N'Kệ A2'),
(2, N'VH001', N'Truyện Kiều', N'Nguyễn Du', 75000, 3, N'Kệ B1');

-- =============================
-- MƯỢN SÁCH
-- =============================
INSERT INTO MuonTra (the_id, DanhMucSach_id, DueDate, TrangThai)
VALUES
(1, 1, DATEADD(DAY,14,GETDATE()), 'open'),
(1, 2, DATEADD(DAY,14,GETDATE()), 'open');

-- =============================
-- PHÒNG HỌC & ĐẶT PHÒNG
-- =============================
INSERT INTO Phong (Ten_Phong, DienTich, SucChua)
VALUES
(N'Phòng 101', 50.5, 20),
(N'Phòng 102', 70.0, 30);

INSERT INTO DatPhong (user_id, Phong_id, GioBatDau, GioKetThuc, TrangThai)
VALUES
(1, 1, DATEADD(HOUR,1,GETDATE()), DATEADD(HOUR,3,GETDATE()), 'pending'),
(2, 2, DATEADD(HOUR,2,GETDATE()), DATEADD(HOUR,4,GETDATE()), 'approved');

-- =============================
-- BỔ SUNG TÀI LIỆU
-- =============================
INSERT INTO BoSungTaiLieu (DeXuat, XuLy, CapNhat, NoiDung, DanhMucSach_id)
VALUES
(N'Đề xuất mua thêm sách AI', N'Đang xét duyệt', N'2025-09-07', N'Tài liệu AI nâng cao', NULL);

-- =============================
-- ĐẶT TRƯỚC SÁCH
-- =============================
INSERT INTO DatTruocSach (DanhMucSach_id, user_id, TrangThai)
VALUES
(3, 1, 'pending'); -- User 1 đặt trước sách Truyện Kiều

-- =============================
-- VI PHẠM USER
-- =============================
INSERT INTO ViPhamUser (user_id, CountNoShow, CountTreHan)
VALUES
(1, 0, 1),  -- User 1 trễ hạn 1 lần
(2, 2, 0);  -- User 2 no-show 2 lần

-- =============================
-- THAM SỐ HỆ THỐNG
-- =============================
INSERT INTO ThamSoHeThong (Ten, GiaTri)
VALUES
(N'PhatMoiNgayTre', N'2000'),  -- phạt 2000đ/ngày trễ
(N'ThoigianMoCua', N'08:00-20:00');
