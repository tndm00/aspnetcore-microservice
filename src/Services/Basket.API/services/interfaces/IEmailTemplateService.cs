namespace Basket.API.services.interfaces
{
    public interface IEmailTemplateService
    {
        string GenerateReminderCheckoutOrderEmail(string email, string username);
    }
}
