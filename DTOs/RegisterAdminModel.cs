namespace ApiWow.DTOs
{
    public class RegisterAdminModel: RegisterModel
    {
        public string Role { get; set; } = "Admin";
    }
}
