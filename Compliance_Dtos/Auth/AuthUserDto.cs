namespace Compliance_Dtos.Auth
{

    public class AuthUserDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }  // FirstName + LastName
        public string Email { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public string PasswordHash { get; set; } // Only for internal verification
        public int Status { get; set; }  // From user_profile.status

    }

}
