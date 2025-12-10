using System;
using ProjectManagement.Domain.Entities.Users;

namespace ProjectManagement.Application.DTOs.Tasks
{
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }                   
        public string Title { get; set; } = null!;    
        public string? Type { get; set; }               
        public string Status { get; set; } = "TO_DO";   
        public string Priority { get; set; } = "MEDIUM"; 

        public Guid ProjectId { get; set; }          

        public Guid? AssignTo { get; set; }         
        public User? User { get; set; }   

        public DateTime? DueDate { get; set; }         

    }
}
