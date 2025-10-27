CREATE DATABASE ThuVienDB
GO
USE ThuVienDB
GO

-- 1. Nhóm sách
CREATE TABLE NhomSach (
  id INT PRIMARY KEY IDENTITY(1,1),
  MaNhom NVARCHAR(6) NOT NULL UNIQUE,
  TenNhom NVARCHAR(50) NOT NULL
);

-- 2. Danh mục sách (đầu sách)
CREATE TABLE DanhMucSach (
  id INT PRIMARY KEY IDENTITY(1,1),
  NhomSach_id INT NOT NULL,
  MaSach NVARCHAR(20) NOT NULL UNIQUE,
  TenSach NVARCHAR(255) NOT NULL,
  TacGia NVARCHAR(255) NOT NULL,
  DonGia DECIMAL(10,2) NOT NULL CHECK (DonGia >= 0),
  SLTon INT NOT NULL CHECK (SLTon >= 0),
  ViTriKe NVARCHAR(50) NOT NULL,
  AnhUrl NVARCHAR(255),
  FOREIGN KEY (NhomSach_id) REFERENCES NhomSach(id)
);

-- 3. Bản thể sách (mỗi quyển riêng biệt)
CREATE TABLE BanTheSach (
  id INT PRIMARY KEY IDENTITY(1,1),
  DanhMucSach_id INT NOT NULL,
  MaVach NVARCHAR(50) NOT NULL UNIQUE,
  TinhTrang NVARCHAR(20) NOT NULL CHECK (TinhTrang IN ('available','borrowed','lost','damaged')),
  FOREIGN KEY (DanhMucSach_id) REFERENCES DanhMucSach(id)
);

-- 4. Vai trò người dùng
CREATE TABLE Role (
  id INT PRIMARY KEY IDENTITY(1,1),
  name NVARCHAR(50) NOT NULL UNIQUE
);

-- 5. Người dùng
CREATE TABLE [User] (
  id INT PRIMARY KEY IDENTITY(1,1),
  fullname NVARCHAR(100) NOT NULL,
  email NVARCHAR(100) UNIQUE NOT NULL,
  phone NVARCHAR(20),
  password NVARCHAR(255) NOT NULL,
  created_at DATETIME DEFAULT GETDATE(),
  role_id INT NOT NULL,
  FOREIGN KEY (role_id) REFERENCES Role(id)
);

-- 6. Thẻ thư viện
CREATE TABLE The (
  id INT PRIMARY KEY IDENTITY(1,1),
  The_code NVARCHAR(20) NOT NULL UNIQUE,
  user_id INT NOT NULL,
  HanThe DATETIME NOT NULL,
  TrangThai NVARCHAR(20) NOT NULL CHECK (TrangThai IN ('active','expired','blocked')),
  FOREIGN KEY (user_id) REFERENCES [User](id)
);

-- 7. Mượn trả sách
CREATE TABLE MuonTra (
  id INT PRIMARY KEY IDENTITY(1,1),
  the_id INT NOT NULL,
  BanTheSach_id INT NOT NULL,
  NgayMuon DATETIME NOT NULL DEFAULT GETDATE(),
  DueDate DATETIME NOT NULL,
  NgayTra DATETIME NULL,
  SoLanGiaHan INT NOT NULL DEFAULT 0 CHECK (SoLanGiaHan >= 0),
  TienPhat DECIMAL(10,2) DEFAULT 0 CHECK (TienPhat >= 0),
  TrangThai NVARCHAR(20) NOT NULL CHECK (TrangThai IN ('open','returned','lost')),
  FOREIGN KEY (the_id) REFERENCES The(id),
  FOREIGN KEY (BanTheSach_id) REFERENCES BanTheSach(id)
);

-- 8. Phòng học
CREATE TABLE Phong (
  id INT PRIMARY KEY IDENTITY(1,1),
  Ten_Phong NVARCHAR(50) NOT NULL,
  DienTich DECIMAL(10,2) NOT NULL CHECK (DienTich > 0),
  SucChua INT NOT NULL CHECK (SucChua > 0)
);

-- 9. Đặt phòng học
CREATE TABLE DatPhong (
  id INT PRIMARY KEY IDENTITY(1,1),
  user_id INT NOT NULL,
  Phong_id INT NOT NULL,
  NgayDat DATETIME NOT NULL DEFAULT GETDATE(),
  GioBatDau DATETIME NOT NULL,
  GioKetThuc DATETIME NOT NULL,
  TrangThai NVARCHAR(20) NOT NULL CHECK (TrangThai IN ('pending','approved','checkedin','noshow','cancelled')),
  FOREIGN KEY (Phong_id) REFERENCES Phong(id),
  FOREIGN KEY (user_id) REFERENCES [User](id)
);

-- 10. Đề xuất bổ sung tài liệu
CREATE TABLE BoSungTaiLieu (
  id INT PRIMARY KEY IDENTITY(1,1),
  DeXuat NVARCHAR(255) NOT NULL,
  XuLy NVARCHAR(100) NOT NULL,
  CapNhat NVARCHAR(100) NOT NULL,
  NoiDung NVARCHAR(255) NOT NULL,
  DanhMucSach_id INT NULL,
  FOREIGN KEY (DanhMucSach_id) REFERENCES DanhMucSach(id)
);

-- 11. Báo cáo vòng quay sách
CREATE TABLE BaoCaoVongQuaySach (
  id INT PRIMARY KEY IDENTITY(1,1),
  DanhMucSach_id INT NOT NULL,
  ThongKeLuotMuon INT NOT NULL CHECK (ThongKeLuotMuon >= 0),
  ThoiGianBaoCao DATETIME NOT NULL DEFAULT GETDATE(),
  FOREIGN KEY (DanhMucSach_id) REFERENCES DanhMucSach(id)
);

-- 12. Hạn mức mượn theo vai trò
CREATE TABLE HanMuc (
  id INT PRIMARY KEY IDENTITY(1,1),
  role_id INT NOT NULL,
  MaxBooks INT NOT NULL CHECK (MaxBooks > 0),
  MaxDays INT NOT NULL CHECK (MaxDays > 0),
  MaxRenewals INT NOT NULL CHECK (MaxRenewals >= 0),
  MaxFines DECIMAL(10,2) NOT NULL CHECK (MaxFines >= 0),
  FOREIGN KEY (role_id) REFERENCES Role(id)
);

-- 13. Đặt trước sách
CREATE TABLE DatTruocSach (
  id INT PRIMARY KEY IDENTITY(1,1),
  DanhMucSach_id INT NOT NULL,
  user_id INT NOT NULL,
  NgayDat DATETIME NOT NULL DEFAULT GETDATE(),
  TrangThai NVARCHAR(20) NOT NULL CHECK (TrangThai IN ('pending','approved','cancelled')),
  FOREIGN KEY (DanhMucSach_id) REFERENCES DanhMucSach(id),
  FOREIGN KEY (user_id) REFERENCES [User](id)
);

-- 14. Vi phạm của người dùng
CREATE TABLE ViPhamUser (
  id INT PRIMARY KEY IDENTITY(1,1),
  user_id INT NOT NULL,
  CountNoShow INT DEFAULT 0,
  CountTreHan INT DEFAULT 0,
  LastUpdated DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (user_id) REFERENCES [User](id)
);

-- 15. Tham số hệ thống
CREATE TABLE ThamSoHeThong (
  id INT PRIMARY KEY IDENTITY(1,1),
  Ten NVARCHAR(100) NOT NULL UNIQUE,
  GiaTri NVARCHAR(100) NOT NULL
);

-- 16. Indexes hỗ trợ tìm kiếm & hiệu năng
CREATE INDEX IX_Sach_TenSach ON DanhMucSach(TenSach);
CREATE INDEX IX_Sach_TacGia ON DanhMucSach(TacGia);
CREATE INDEX IX_MuonTra_The_Open ON MuonTra(the_id) WHERE TrangThai = 'open';
CREATE INDEX IX_DatPhong_Phong ON DatPhong(Phong_id, GioBatDau, GioKetThuc);
