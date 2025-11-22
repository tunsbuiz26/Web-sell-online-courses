using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    [Table("BaiHoc")]
    public class BaiHoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KhoaHocId { get; set; }

        [Required, StringLength(200)]
        public string? TenBaiHoc { get; set; }

        [Required, StringLength(20)]
        public string LoaiNoiDung { get; set; } = "Video";

        public string? DuongDanNoiDung { get; set; }
        public int ThuTuHienThi { get; set; } = 0;

    
        [ForeignKey("KhoaHocId")]
        public virtual KhoaHoc? KhoaHoc { get; set; }

        public virtual ICollection<TienDoHocTap>? TienDoHocTap { get; set; }

    }

}
