//using Domain.Entities;
//using Domain.Events;
//using MediatR;

//namespace Application.EventHandlers
//{
//    public class PositionCreatedEventHandler : INotificationHandler<PositionCreatedEvent>
//    {
//        public Task Handle(PositionCreatedEvent notification, CancellationToken cancellationToken)
//        {
//            Console.WriteLine($"Event handler: {notification.CreateDateTime}");

//            var position = new Position
//            {
//                Id = notification.Id,
//                Latitude = notification.Latitude,
//                Longitude = notification.Longitude,
//                Height = notification.Height,
//                CreatedDateTime = notification.CreateDateTime
//            };

//            return Task.CompletedTask;
//        }
//    }
//}
