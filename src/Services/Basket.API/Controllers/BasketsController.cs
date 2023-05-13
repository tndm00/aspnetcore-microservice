using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using Basket.API.services.interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly StockItemGrpcService _stockItemGrpcService;
        private readonly IEmailTemplateService _emailTemplateService;

        public BasketsController(IBasketRepository basketRepository, 
            IPublishEndpoint publishEndpoint, IMapper mapper,
            StockItemGrpcService stockItemGrpcService,
            IEmailTemplateService emailTemplateService)
        {
            _basketRepository = basketRepository ?? throw new ArgumentException(nameof(basketRepository));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentException(nameof(publishEndpoint));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _stockItemGrpcService = stockItemGrpcService ?? throw new ArgumentException(nameof(stockItemGrpcService));
            _emailTemplateService = emailTemplateService;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetBasketByUserName([Required] string userName)
        {
            var result = await _basketRepository.GetBasketByUserName(userName);
            return Ok(result ?? new Cart());
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdataBasket([FromBody] Cart cart)
        {
            //Communicate with Inventory
            foreach (var item in cart.Items)
            {
                var stock = await _stockItemGrpcService.GetStock(item.ItemNo);
                item.SetAvailableQuantity(stock.Quantity);
            }

            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            var result = await _basketRepository.UpdateBasket(cart, options);
            return Ok(result);
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteBasket([Required] string userName)
        {
            var result = await _basketRepository.DeleteBasketFromUserName(userName);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckOut basketCheckout)
        {
            var basket = await _basketRepository.GetBasketByUserName(basketCheckout.UserName);
            if (basket == null) return NotFound();

            //Publish checkout event to EventBus Message
            var eventMessage = _mapper.Map<BasketCheckOutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            //remove the basket
            await _basketRepository.DeleteBasketFromUserName(basketCheckout.UserName);

            return Accepted();
        }

        [HttpPost("[action]", Name = "SendEmailReminder")]
        public ContentResult SendEmailReminder()
        {
            var emailTemplate = _emailTemplateService.GenerateReminderCheckoutOrderEmail("tndm@gmail.com", "tndm00");

            var result = new ContentResult
            {
                Content = emailTemplate,
                ContentType = "text/html"
            };

            return result;
        }
    }
}
