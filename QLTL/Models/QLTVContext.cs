using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QLTV_Backend.Models
{
    public class QLTVContext : DbContext
    {
        public DbSet<NhomSach> NhomSaches { get; set; }
        public DbSet<DanhMucSach> DanhMucSaches { get; set; }
        public DbSet<BoSungTaiLieu> BoSungTaiLieus { get; set; }
        public DbSet<BaoCaoVongQuaySach> BaoCaoVongQuaySaches { get; set; }

        // Thêm các bảng khác nếu cần
    }
}
