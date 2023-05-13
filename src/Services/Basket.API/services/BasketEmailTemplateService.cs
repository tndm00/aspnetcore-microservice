﻿using Basket.API.services.interfaces;

namespace Basket.API.services
{
    public class BasketEmailTemplateService : EmailTemplateService, IEmailTemplateService
    {
        public string GenerateReminderCheckoutOrderEmail(string email, string username)
        {
            var checkoutUrl = "http//localhost:5001/baskets/checkout";
            var emailText = ReadEmailTemplateContent("reminder-checkout-order");
            var emailReplaceText = emailText.Replace("[username]", username)
                .Replace("[checkoutUrl]", checkoutUrl);

            return emailReplaceText;
        }
    }
}
