namespace ProjectManagement.Application.DTOs.Users
{
    public class RegisterDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;       // invitation password（原数据库密码）
    public string NewPassword { get; set; } = null!;    // 用户设置的新密码
    public string ConfirmPassword { get; set; } = null!;// 前端验证用
    public string Name { get; set; } = null!;           // 用户修改姓名
   // public string? Role { get; set; } = null;        // 用户选择角色
}

}
