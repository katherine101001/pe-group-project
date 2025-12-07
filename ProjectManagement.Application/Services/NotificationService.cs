using AutoMapper;
using ProjectManagement.Application.DTOs.Collaboration;
using ProjectManagement.Application.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Shared.Exceptions;

namespace ProjectManagement.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<NotificationDto> AddNotificationAsync(NotificationDto dto)
        {
            // Map DTO -> Entity
            var notification = _mapper.Map<Notification>(dto);

            // Save entity
            await _notificationRepository.AddAsync(notification);

            // Map Entity -> DTO
            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<NotificationDto?> GetNotificationByIdAsync(Guid id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);

            if (notification == null)
                return null;

            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<List<NotificationDto>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task UpdateNotificationAsync(Guid id, NotificationDto dto)
        {
            var existingNotification = await _notificationRepository.GetByIdAsync(id);

            if (existingNotification == null)
                throw new NotFoundException("Notification not found");

            // Map updated values from DTO to entity
            _mapper.Map(dto, existingNotification);

            await _notificationRepository.UpdateAsync(existingNotification);
        }

        public async Task DeleteNotificationAsync(Guid id)
        {
            var existingNotification = await _notificationRepository.GetByIdAsync(id);

            if (existingNotification == null)
                throw new NotFoundException("Notification not found");

            await _notificationRepository.DeleteAsync(existingNotification);
        }
    }
}
