namespace Compliance_Dtos.Auth
{
    public class UserStatusDto
    {
        public string UserId { get; set; }
        public int Status { get; set; }             // true = Active, false = Inactive
    }
}
