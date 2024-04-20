using System.Diagnostics;
using BookStore.Interfaces;

namespace BookStore.Messages;

public class DebugNotificationService : INotificationService
{
    public void SendConfirmationCode(string cellPhone, int code)
    {
        Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);
    }
}