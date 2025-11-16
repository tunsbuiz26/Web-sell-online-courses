using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    [Table("DangKyKhoaHocs")]
    public class DangKyKhoaHoc
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("KhoaHoc")]
        public int KhoaHocID { get; set; }
        public DateTime NgayDangKy { get; set; }
        public string? TrangThai { get; set; }
        public User? User { get; set; }
        public KhoaHoc? KhoaHoc { get; set; }
    }
}
