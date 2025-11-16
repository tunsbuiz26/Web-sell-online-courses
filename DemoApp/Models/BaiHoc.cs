using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace DemoApp.Models
{
    [Table("BaiHocs")]
    public class BaiHoc
    {
        [Key]
        public int BaiHocID { get; set; }
        [ForeignKey("KhoaHoc")]
        public int KhoaHocID { get; set; }
        public string? TieuDe { get; set; }
        public string? NoiDung { get; set; }
        public int ThuTu { get; set; }
        public int ThoiLuong { get; set; }
        public KhoaHoc? KhoaHoc { get; set; }
        public ICollection<TienDoHocTap>? tienDoHoctaps { get; set; }
    }
}
