using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QL_ThuVien.Data
{
    public class QLThuVienDbContext : IdentityDbContext
    {
        public QLThuVienDbContext(DbContextOptions<QLThuVienDbContext> options)
            : base(options)
        {
        }
    }
}
