using System;

namespace ProjectManagement.Application.DTOs.Projects
{
    public class SearchProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int Progress { get; set; }
    }
}
