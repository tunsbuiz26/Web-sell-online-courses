using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    [Table("KhoaHoc")]
    public class KhoaHoc
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string MaKhoaHoc { get; set; }

        [Required, StringLength(200)]
        public string TenKhoaHoc { get; set; }

        public string? MoTaNgan { get; set; }
        public string? AnhBia { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? DanhMucId { get; set; }

        [Required, StringLength(20)]
        public string CapDo { get; set; } = "CoBan";

        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaTien { get; set; } = 0;

        [Required, StringLength(20)]
        public string TrangThai { get; set; } = "BanNhap";

        public DateTime NgayTao { get; set; } = DateTime.Now;

        
        [ForeignKey("UserId ")]
        public virtual User? user { get; set; }

        [ForeignKey("DanhMucId")]
        public virtual DanhMuc? DanhMuc { get; set; }

        public virtual ICollection<BaiHoc>? BaiHoc { get; set; }
        public virtual ICollection<DangKyKhoaHoc>? DangKyKhoaHoc { get; set; }
    }
}
