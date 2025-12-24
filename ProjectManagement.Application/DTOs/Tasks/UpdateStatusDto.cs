using System;
using ProjectManagement.Domain.Entities.Users;

namespace ProjectManagement.Application.DTOs.Tasks
{
    public class UpdateTaskStatusDto
    {
        public string Status { get; set; } = null!;
    }
}
