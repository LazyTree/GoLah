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
            IEnumerable<ArrivalBusService> arrivalBuses;
            if (DateTime.Now.Hour < 18) return;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var busStopCode = localSettings.Values["busStopCode"].ToString();
            var notificationMessage = new StringBuilder();
            if (!string.IsNullOrEmpty(busStopCode))
            {
                arrivalBuses = await new LtaDataRepositoryBase<ArrivalBusService>().QueryAsync(true, busStopCode);
                if (arrivalBuses.Any())
                {
                    arrivalBuses.ToList().ForEach(x => notificationMessage.AppendLine(x.ToString()));
                }
            }

            var notifier = ToastNotificationManager.CreateToastNotifier();
            var content = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var text = content.GetElementsByTagName("text");
            text[0].InnerText = $"BusStop {busStopCode}";
            text[1].InnerText = notificationMessage.ToString();
            notifier.Show(new ToastNotification(content));
        }

        public static async void Register()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["busStopCode"] = "53121";

            var isRegistered = BackgroundTaskRegistration.AllTasks.Values.Any(t => t.Name == nameof(GoLahBackgroundTask));
            if (isRegistered) return;

            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.DeniedBySystemPolicy || result == BackgroundAccessStatus.DeniedByUser) return;

            var builder = new BackgroundTaskBuilder()
            {
                Name = nameof(GoLahBackgroundTask),
                TaskEntryPoint = $"{nameof(GoLah)}.{nameof(BackgroundService)}.{nameof(GoLahBackgroundTask)}"
            };
            builder.SetTrigger(new TimeTrigger(3, false));
            builder.Register();
        }
    }
}
