using System;

namespace ProjectManagement.Application.DTOs.Tasks
{
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }                     // 任务ID
        public string Title { get; set; } = null!;      // 任务标题
        public string? Type { get; set; }               // 任务类型，可选
        public string Status { get; set; } = "TO_DO";   // 任务状态
        public string Priority { get; set; } = "MEDIUM"; // 优先级

        public Guid ProjectId { get; set; }             // 所属项目ID

        public Guid? AssignTo { get; set; }             // 分配的用户ID
        public string? AssigneeName { get; set; }       // 用户名（只读，用于显示）

        public DateTime? DueDate { get; set; }          // 截止日期
        //public DateTime CreatedAt { get; set; }         // 创建时间
       // public DateTime? UpdatedAt { get; set; }        // 更新时间
    }
}
