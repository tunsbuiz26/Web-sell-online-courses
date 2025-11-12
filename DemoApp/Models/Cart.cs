namespace DemoApp.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
