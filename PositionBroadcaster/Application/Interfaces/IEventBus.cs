﻿using IntergrationEvents;

namespace Application.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : IntegrationEventBase;
    }
}