namespace ShoesShoppingOnline.DTO.Request.Users
{
    public class UserRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        public string? AvatarImage { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int RoleId { get; set; }
    }
}
