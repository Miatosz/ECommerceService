namespace ECommerceService.Api.Dto
{
    public class CreateUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Country { get; set; }
        public string Password { get; set; } = null!;
    }
}
