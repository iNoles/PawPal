using PawPal.Models;
using Plugin.LocalNotification;

namespace PawPal.Services;

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

    public static async Task ScheduleNotificationAsync(PetTask task)
    {
        if (task.ScheduledDate > DateTime.Now) // Only schedule for future dates
        {
            var notification = new NotificationRequest
            {
                NotificationId = task.Id, // Unique ID for the task
                Title = "Task Reminder",
                Description = $"Task '{task.Title}' is due soon!",
                ReturningData = $"TaskId:{task.Id}",
                Schedule =
                {
                    NotifyTime = task.ScheduledDate // Schedule for the task's due date
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
        }
    }

    public static async Task ScheduleRecurringNotifications(PetTask task)
    {
        DateTime currentNotificationTime = task.ScheduledDate;

        // Loop through the recurrence interval and schedule notifications until the end date
        while (currentNotificationTime <= task.EndDate)
        {
            // Schedule the notification
            task.ScheduledDate = currentNotificationTime;
            await ScheduleNotificationAsync(task);

            // Calculate the next recurrence date based on recurrence type and interval
            currentNotificationTime = GetNextRecurrenceTime(currentNotificationTime, task.RecurrenceType, task.RecurrenceInterval);
        }

        // Once the task has reached its end date or the recurrence period has passed, drop the scheduling
        Console.WriteLine("Task recurrence has ended or exceeded the expected period. No further notifications will be scheduled.");
    }

    private static DateTime GetNextRecurrenceTime(DateTime currentNotificationTime, string recurrenceType, int recurrenceInterval)
    {
        // Add validation for recurrence interval
        if (recurrenceInterval <= 0)
        {
            Console.WriteLine("Recurrence interval should be a positive number.");
            return currentNotificationTime;
        }
        return recurrenceType switch
        {
            "Daily" => currentNotificationTime.AddDays(recurrenceInterval),
            "Weekly" => currentNotificationTime.AddDays(7 * recurrenceInterval),
            "Monthly" => currentNotificationTime.AddMonths(recurrenceInterval),
            _ => currentNotificationTime,
        };
    }
}
