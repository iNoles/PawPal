using Plugin.LocalNotification;

namespace PawPal;

public class NotificationService
{
    public static void SchedulePetActivityReminder(string taskName, DateTime dueDate, string petName)
    {
        // Create a notification
        var notification = new NotificationRequest
        {
            Title = $"{petName} - {taskName}",
            Description = $"Don't forget to {taskName} for {petName}.",
            NotificationId = new Random().Next(1, 1000), // Random ID for each task
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = dueDate // The time when the notification should appear
            }
        };

        // Schedule the notification
        LocalNotificationCenter.Current.Show(notification);
    }

    public static void CancelReminder(int notificationId)
    {
        LocalNotificationCenter.Current.Cancel(notificationId);
    }
}
