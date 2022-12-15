using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckOut, BasketCheckOutEvent>();
        }
    }
}
