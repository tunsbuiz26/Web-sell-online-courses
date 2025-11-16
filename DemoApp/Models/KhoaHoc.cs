using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    [Table("KhoaHocs")]
    public class KhoaHoc
    {
        [Key]
        public int KhoaHocID { get; set; }
        public string? TenKhoaHoc { get; set; }
        public string? MoTa { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime NgayTao { get; set; }
        public bool TrangThai { get; set; }

        public ICollection<BaiHoc>? BaiHocs { get; set; }

        public ICollection<DangKyKhoaHoc>? dangKyKhoaHocs { get; set; }
    }
}
