namespace BookStore.Interfaces;

public interface INotificationService
{
    void SendConfirmationCode(string cellPhone, int code);
}