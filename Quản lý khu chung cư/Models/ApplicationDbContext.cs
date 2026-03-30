using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Quản_lý_khu_chung_cư.Models
{
    public class ApplicationDbContext : IdentityDbContext<NguoiDung>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TienIch> TienIch { get; set; }
        public DbSet<BietThu> BietThu { get; set; }
        public DbSet<CanHo> CanHo { get; set; }
        public DbSet<DatTienIch> DatTienIch { get; set; }
        public DbSet<ErrorViewModel> ErrorViewModel { get; set; }
        public DbSet<LichBaoTri> LichBaoTri { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<PhanAnh> PhanAnh { get; set; }
        public DbSet<PhiDichVu> PhiDichVu { get; set; }
        public DbSet<RaVao> RaVao { get; set; }
        public DbSet<ThongBao> ThongBao { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }      
       
    }
}
