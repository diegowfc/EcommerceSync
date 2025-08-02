namespace Application.DTOs.UserDtos
{
    public class UserCreateDto
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}
