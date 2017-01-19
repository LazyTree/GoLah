using GoLah.Model;
using GoLah.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace GoLah.BackgroundService
{
    public sealed class GoLahBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            await SendNotification();
            _deferral.Complete();
        }

        private async Task SendNotification()
        {
            var repo = new LtaDataRepository();
            var buses = await repo.GetBusRoutesAsync(true);
            var stops = await repo.GetBusStopsAsync(true);
            var routeStops = repo.CachedRouteStops;

            var notifier = ToastNotificationManager.CreateToastNotifier();
            var content = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var text = content.GetElementsByTagName("text");
            text[0].InnerText = $"There are {buses.Count()} buses and {stops.Count()} stops and {routeStops.Count()} route stops retrieved.";
            notifier.Show(new ToastNotification(content));
        }

        public static async void Register()
        {
            var isRegistered = BackgroundTaskRegistration.AllTasks.Values.Any(t => t.Name == nameof(GoLahBackgroundTask));
            if (isRegistered) return;

            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.DeniedBySystemPolicy || result == BackgroundAccessStatus.DeniedByUser) return;

            var builder = new BackgroundTaskBuilder()
            {
                Name = nameof(GoLahBackgroundTask),
                TaskEntryPoint = $"{nameof(GoLah)}.{nameof(BackgroundService)}.{nameof(GoLahBackgroundTask)}"
            };
            builder.SetTrigger(new TimeTrigger(120, false));
            builder.Register();
        }
    }
}
