using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QL_ThuVien.Models;

namespace QL_ThuVien.Data
{
    public class QL_ThuVienContext : DbContext
    {
        public QL_ThuVienContext (DbContextOptions<QL_ThuVienContext> options)
            : base(options)
        {
        }

        public DbSet<QL_ThuVien.Models.User> User { get; set; } = default!;
        public DbSet<QL_ThuVien.Models.NhomSach> NhomSach { get; set; } = default!;
        public DbSet<QL_ThuVien.Models.DanhMucSach> DanhMucSach { get; set; } = default!;
        public DbSet<QL_ThuVien.Models.BanTheSach> BanTheSach { get; set; } = default!;
        public DbSet<QL_ThuVien.Models.DatTruocSach> DatTruocSach { get; set; } = default!;
    }
}
