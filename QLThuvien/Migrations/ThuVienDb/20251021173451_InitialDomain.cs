using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLThuvien.Migrations.ThuVienDb
{
    /// <inheritdoc />
    public partial class InitialDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NhomSach",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhom = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    TenNhom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhomSach__3213E83F7EE02729", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Phong",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DienTich = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SucChua = table.Column<int>(type: "int", nullable: false),
                    LoaiPhong = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Phong__3213E83F169ACA9C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3213E83F8B0E788F", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ThamSoHeThong",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiaTri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThamSoHe__3213E83F18B7E309", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucSach",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NhomSach_id = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenSach = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TacGia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SLTon = table.Column<int>(type: "int", nullable: false),
                    ViTriKe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DanhMucS__3213E83FD6019CF9", x => x.id);
                    table.ForeignKey(
                        name: "FK__DanhMucSa__NhomS__3D5E1FD2",
                        column: x => x.NhomSach_id,
                        principalTable: "NhomSach",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HanMuc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    MaxBooks = table.Column<int>(type: "int", nullable: false),
                    MaxDays = table.Column<int>(type: "int", nullable: false),
                    MaxRenewals = table.Column<int>(type: "int", nullable: false),
                    MaxFines = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HanMuc__3213E83F8306B867", x => x.id);
                    table.ForeignKey(
                        name: "FK__HanMuc__role_id__6FE99F9F",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3213E83F72144F6C", x => x.id);
                    table.ForeignKey(
                        name: "FK__User__role_id__49C3F6B7",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BanTheSach",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DanhMucSach_id = table.Column<int>(type: "int", nullable: false),
                    MaVach = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BanTheSa__3213E83F3B3E4872", x => x.id);
                    table.ForeignKey(
                        name: "FK__BanTheSac__DanhM__4222D4EF",
                        column: x => x.DanhMucSach_id,
                        principalTable: "DanhMucSach",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoVongQuaySach",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DanhMucSach_id = table.Column<int>(type: "int", nullable: false),
                    ThongKeLuotMuon = table.Column<int>(type: "int", nullable: false),
                    ThoiGianBaoCao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BaoCaoVo__3213E83FD454339F", x => x.id);
                    table.ForeignKey(
                        name: "FK__BaoCaoVon__DanhM__693CA210",
                        column: x => x.DanhMucSach_id,
                        principalTable: "DanhMucSach",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BoSungTaiLieu",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeXuat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    XuLy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CapNhat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DanhMucSach_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BoSungTa__3213E83FB2FE54A4", x => x.id);
                    table.ForeignKey(
                        name: "FK__BoSungTai__DanhM__6477ECF3",
                        column: x => x.DanhMucSach_id,
                        principalTable: "DanhMucSach",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DatPhong",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    Phong_id = table.Column<int>(type: "int", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    GioBatDau = table.Column<DateTime>(type: "datetime", nullable: false),
                    GioKetThuc = table.Column<DateTime>(type: "datetime", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DatPhong__3213E83FAAEB3603", x => x.id);
                    table.ForeignKey(
                        name: "FK__DatPhong__Phong___60A75C0F",
                        column: x => x.Phong_id,
                        principalTable: "Phong",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DatPhong__user_i__619B8048",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DatTruocSach",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DanhMucSach_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DatTruoc__3213E83F5D2CF91A", x => x.id);
                    table.ForeignKey(
                        name: "FK__DatTruocS__DanhM__74AE54BC",
                        column: x => x.DanhMucSach_id,
                        principalTable: "DanhMucSach",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DatTruocS__user___75A278F5",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "The",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    The_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    HanThe = table.Column<DateTime>(type: "datetime", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__The__3213E83FA205BDF9", x => x.id);
                    table.ForeignKey(
                        name: "FK__The__user_id__4E88ABD4",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ViPhamUser",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    CountNoShow = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    CountTreHan = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ViPhamUs__3213E83FD1117D98", x => x.id);
                    table.ForeignKey(
                        name: "FK__ViPhamUse__user___7B5B524B",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MuonTra",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    the_id = table.Column<int>(type: "int", nullable: false),
                    BanTheSach_id = table.Column<int>(type: "int", nullable: false),
                    NgayMuon = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    NgayTra = table.Column<DateTime>(type: "datetime", nullable: true),
                    SoLanGiaHan = table.Column<int>(type: "int", nullable: false),
                    TienPhat = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0m),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MuonTra__3213E83F0D752CBC", x => x.id);
                    table.ForeignKey(
                        name: "FK__MuonTra__BanTheS__5812160E",
                        column: x => x.BanTheSach_id,
                        principalTable: "BanTheSach",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__MuonTra__the_id__571DF1D5",
                        column: x => x.the_id,
                        principalTable: "The",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BanTheSach_DanhMucSach_id",
                table: "BanTheSach",
                column: "DanhMucSach_id");

            migrationBuilder.CreateIndex(
                name: "UQ__BanTheSa__8BBF4A1C11D752B6",
                table: "BanTheSach",
                column: "MaVach",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoVongQuaySach_DanhMucSach_id",
                table: "BaoCaoVongQuaySach",
                column: "DanhMucSach_id");

            migrationBuilder.CreateIndex(
                name: "IX_BoSungTaiLieu_DanhMucSach_id",
                table: "BoSungTaiLieu",
                column: "DanhMucSach_id");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucSach_NhomSach_id",
                table: "DanhMucSach",
                column: "NhomSach_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_TacGia",
                table: "DanhMucSach",
                column: "TacGia");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_TenSach",
                table: "DanhMucSach",
                column: "TenSach");

            migrationBuilder.CreateIndex(
                name: "UQ__DanhMucS__B235742C011C7345",
                table: "DanhMucSach",
                column: "MaSach",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_Phong",
                table: "DatPhong",
                columns: new[] { "Phong_id", "GioBatDau", "GioKetThuc" });

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_user_id",
                table: "DatPhong",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_DatTruocSach_DanhMucSach_id",
                table: "DatTruocSach",
                column: "DanhMucSach_id");

            migrationBuilder.CreateIndex(
                name: "IX_DatTruocSach_user_id",
                table: "DatTruocSach",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_HanMuc_role_id",
                table: "HanMuc",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_MuonTra_BanTheSach_id",
                table: "MuonTra",
                column: "BanTheSach_id");

            migrationBuilder.CreateIndex(
                name: "IX_MuonTra_The_Open",
                table: "MuonTra",
                column: "the_id",
                filter: "([TrangThai]='open')");

            migrationBuilder.CreateIndex(
                name: "UQ__NhomSach__234F91CC048CCA06",
                table: "NhomSach",
                column: "MaNhom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Role__72E12F1B162A7D2C",
                table: "Role",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ThamSoHe__C451FA839DD50A7D",
                table: "ThamSoHeThong",
                column: "Ten",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_The_user_id",
                table: "The",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__The__F4DC115952C80D47",
                table: "The",
                column: "The_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__User__AB6E6164424CEEE9",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViPhamUser_user_id",
                table: "ViPhamUser",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoVongQuaySach");

            migrationBuilder.DropTable(
                name: "BoSungTaiLieu");

            migrationBuilder.DropTable(
                name: "DatPhong");

            migrationBuilder.DropTable(
                name: "DatTruocSach");

            migrationBuilder.DropTable(
                name: "HanMuc");

            migrationBuilder.DropTable(
                name: "MuonTra");

            migrationBuilder.DropTable(
                name: "ThamSoHeThong");

            migrationBuilder.DropTable(
                name: "ViPhamUser");

            migrationBuilder.DropTable(
                name: "Phong");

            migrationBuilder.DropTable(
                name: "BanTheSach");

            migrationBuilder.DropTable(
                name: "The");

            migrationBuilder.DropTable(
                name: "DanhMucSach");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "NhomSach");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
