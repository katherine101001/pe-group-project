// 接收前端请求
public class InviteMemberDto
{
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}

// 返回给前端
public class InvitationResponseDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool IsAccepted { get; set; }
    public DateTime CreatedAt { get; set; }
}
