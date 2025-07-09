namespace Compliance_Dtos.Auth
{
    public class GetUsersFilterDto
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Designation { get; set; }
        public string? Location { get; set; }
        public int? RoleId { get; set; }
        public byte? Status { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
