//using Domain.Events;
//using Microsoft.Extensions.DependencyInjection;

//namespace Application.EventHandlers
//{
//    internal class EventHandlerMediator
//    {
//        private readonly Dictionary<string, List<Type>> _handlers;
//        private readonly List<Type> _eventTypes;
//        private readonly IServiceScopeFactory _serviceScopeFactory;

//        public EventHandlerMediator(IServiceScopeFactory serviceScopeFactory)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//            _handlers = new Dictionary<string, List<Type>>();
//            _eventTypes = new List<Type>();
//        }

//        public void Subscribe<T, TH>()
//            where T : EventBase
//            where TH : IEventHandler<T>
//        {
//            var eventName = typeof(T).Name;
//            var handlerType = typeof(TH);

//            if (!_eventTypes.Contains(typeof(T)))
//            {
//                _eventTypes.Add(typeof(T));
//            }

//            if (!_handlers.ContainsKey(eventName))
//            {
//                _handlers.Add(eventName, new List<Type>());
//            }

//            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
//            {
//                throw new ArgumentException(
//                    $"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
//            }

//            _handlers[eventName].Add(handlerType);
//        }

//        private async Task ProcessEvent(string eventName, string message)
//        {
//            if (_handlers.ContainsKey(eventName))
//            {
//                using (var scope = _serviceScopeFactory.CreateScope())
//                {
//                    var subscriptions = _handlers[eventName];
//                    foreach (var subscription in subscriptions)
//                    {
//                        var handler = scope.ServiceProvider.GetService(subscription);
//                        if (handler == null) continue;
//                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
//                        var @event = JsonConvert.DeserializeObject(message, eventType);
//                        var conreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
//                        await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
//                    }
//                }
//            }
//        }
//    }
//}