namespace ProjectManagement.Application.DTOs.Users
{
    public class InviteTeamDto
    {
        public string Email { get; set; } = null!;

        public string? Role { get; set; } = "Member";
    
    }
}