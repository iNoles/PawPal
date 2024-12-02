using PawPal.Models;
using Plugin.LocalNotification;

namespace PawPal;

public class NotificationService
{
    public static async Task InitializeNotificationsAsync()
    {
        if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
        {
            // Request permission if not granted
            var permissionGranted = await LocalNotificationCenter.Current.RequestNotificationPermission();
            if (!permissionGranted)
            {
                Console.WriteLine("Notification permission denied.");
            }
        }
    }

    public static async Task ScheduleNotificationAsync(Tasks task)
    {
        if (task.DueDate > DateTime.Now) // Only schedule for future dates
        {
            var notification = new NotificationRequest
            {
                NotificationId = task.Id, // Unique ID for the task
                Title = "Task Reminder",
                Description = $"Task '{task.TaskName}' is due soon!",
                ReturningData = $"TaskId:{task.Id}",
                Schedule =
                {
                    NotifyTime = task.DueDate // Schedule for the task's due date
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
        }
    }
}
