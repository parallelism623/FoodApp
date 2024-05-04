using FoodShop.Application.Common.Notifications;
using FoodShop.Contract.Abstraction.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Graph.Models;

namespace FoodShop.Infrastructure.Notification
{
    public class NotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        public NotificationSender(IHubContext<NotificationHub> notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public async Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken = default)
            => await _notificationHub.Clients.All.SendAsync(NotificationConstrant.FromServerToClient, notification, cancellationToken);
        public async Task SendToAllAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken)
            => await _notificationHub.Clients.AllExcept(excludedConnectionIds).SendAsync(NotificationConstrant.FromServerToClient, notification, cancellationToken);
        public async Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken)
            => await _notificationHub
            .Clients
            .Group(NotificationConstrant.NormalGroup)
            .SendAsync(NotificationConstrant.FromServerToClient, notification, cancellationToken);


        public async Task SendToGroupAsync(INotificationMessage notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken)
            => await _notificationHub
            .Clients
            .GroupExcept(NotificationConstrant.NormalGroup, excludedConnectionIds)
            .SendAsync(NotificationConstrant.FromServerToClient, notification, cancellationToken);

        public async Task SendToGroupsAsync(INotificationMessage notification, IEnumerable<string> groupNames, CancellationToken cancellationToken)
            => await _notificationHub
            .Clients
            .Groups(groupNames)
            .SendAsync(NotificationConstrant.FromServerToClient, notification, cancellationToken);

        public async Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken)
            => await _notificationHub
            .Clients
            .User(userId)
            .SendAsync(NotificationConstrant.FromServerToClient, notification, cancellationToken);

        public async Task SendToUsersAsync(INotificationMessage notification, IEnumerable<string> userIds, CancellationToken cancellationToken)
            => await _notificationHub
            .Clients
            .Users(userIds)
            .SendAsync(NotificationConstrant.FromServerToClient, notification, cancellationToken);
    }
}
