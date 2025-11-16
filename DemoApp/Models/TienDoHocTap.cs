using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    [Table("TienDoHocTaps")]
    public class TienDoHocTap
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("BaiHoc")]
        public int BaiHocID { get; set; }
        public string? TrangThai { get; set; }
        public DateTime ThoiGianHocCuoi { get; set; }
        public float DiemSo { get; set; }

        public User? User { get; set; }
        public BaiHoc? BaiHoc { get; set; }
    }
}
