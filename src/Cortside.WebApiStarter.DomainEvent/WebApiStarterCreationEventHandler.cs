using System;
using System.Threading.Tasks;
using Cortside.DomainEvent;
using Cortside.DomainEvent.Events;
using Cortside.WebApiStarter.DomainService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Cortside.WebApiStarter.DomainEvent {
    /// <summary>
    /// Handles domain event <see cref="WebApiStarterCreationEvent"/>
    /// </summary>
    public class WebApiStarterCreationEventHandler : IDomainEventHandler<WebApiStarterCreationEvent> {

        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<WebApiStarterCreationEventHandler> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public WebApiStarterCreationEventHandler(IServiceProvider serviceProvider, ILogger<WebApiStarterCreationEventHandler> logger) {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }
        public async Task<HandlerResult> HandleAsync(DomainEventMessage<WebApiStarterCreationEvent> @event) {
            using (LogContext.PushProperty("MessageId", @event.MessageId))
            using (LogContext.PushProperty("CorrelationId", @event.CorrelationId))
            using (LogContext.PushProperty("Parameter", @event.Data.Parameter)) {
                logger.LogDebug($"Handling {typeof(WebApiStarterCreationEvent).Name} for WebApiStarter {@event.Data.Parameter}");

                using (IServiceScope scope = serviceProvider.CreateScope()) {
                    IWebApiStarterService WebApiStarterService = scope.ServiceProvider.GetRequiredService<IWebApiStarterService>();
                    await WebApiStarterService.CreateWebApiStarter(@event.Data.Parameter);
                }

                logger.LogDebug($"Successfully handled {typeof(WebApiStarterCreationEvent).Name} for WebApiStarter {@event.Data.Parameter}");
                return HandlerResult.Success;
            }
        }
    }
}
