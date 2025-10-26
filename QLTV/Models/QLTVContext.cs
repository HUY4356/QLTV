using System.Data.Entity;

namespace QLTV.Models
{
    public class QLTVContext : DbContext
    {
        public QLTVContext() : base("name=QLTVConnectionString") { }

        public DbSet<NhomSach> NhomSachs { get; set; }
        public DbSet<DanhMucSach> DanhMucSachs { get; set; }
        public DbSet<BoSungTaiLieu> BoSungTaiLieus { get; set; }
        public DbSet<BaoCaoVongQuaySach> BaoCaoVongQuaySaches { get; set; }
    }
}
