using ProjectManagement.Application.DTOs.Collaboration;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<NotificationDto> AddNotificationAsync(NotificationDto dto);
        Task<NotificationDto?> GetNotificationByIdAsync(Guid id);
        Task<List<NotificationDto>> GetAllNotificationsAsync();
        Task UpdateNotificationAsync(Guid id, NotificationDto dto);
        Task DeleteNotificationAsync(Guid id);
    }
}
