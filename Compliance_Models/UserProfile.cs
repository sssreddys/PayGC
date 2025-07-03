namespace Compliance_models
{

    public class UserProfile
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public string Gender { get; set; }
        public byte[] ProfileImage { get; set; }
        public string AddedBy { get; set; }
    }

}
