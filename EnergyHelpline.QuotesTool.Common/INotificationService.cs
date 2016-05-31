using EnergyHelpline.QuotesTool.Common.Models;
using System.Threading.Tasks;

namespace EnergyHelpline.QuotesTool.Common
{
    public interface INotificationService
    {
        Task SendNotification(Notification notification);
    }
}
