using FoodShop.Contract.Abstraction.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.Notifications
{
    public interface INotificationSender
    {
        Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken = default);
        Task SendToAllAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken = default);
        Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken);
        Task SendToGroupAsync(INotificationMessage notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken = default);
        Task SendToGroupsAsync(INotificationMessage notification, IEnumerable<string> groupNames, CancellationToken cancellationToken = default);
        Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken = default);
        Task SendToUsersAsync(INotificationMessage notification, IEnumerable<string> userIds, CancellationToken cancellationToken = default);
    }   
}
