namespace ECommerceService.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Country { get; set; }
        public string? PasswordHash { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
