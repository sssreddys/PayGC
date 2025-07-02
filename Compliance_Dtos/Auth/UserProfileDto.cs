namespace Compliance_Dtos.Auth
{
    public class UserProfileDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public byte[] ProfileImage { get; set; }  // ✅ As byte array
    }

}
