using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Cortside.DomainEvent;

namespace Cortside.WebApiStarter.WebApi.IntegrationTests.Helpers {
    public class DomainEventPublisherStub : IDomainEventPublisher {
        public DomainEventError Error { get; set; }

        public event PublisherClosedCallback Closed;

        private readonly List<object> queue = new List<object>();

        public ReadOnlyCollection<object> Queue {
            get {
                return queue.AsReadOnly();
            }
        }

        public void ClearQueue() {
            queue.Clear();
        }

        public Task PublishAsync<T>(T @event) where T : class {
            queue.Add(@event);
            Closed?.Invoke(this, null);
            return Task.CompletedTask;
        }

        public Task PublishAsync<T>(T @event, string correlationId) where T : class {
            queue.Add(@event);
            Closed?.Invoke(this, null);
            return Task.CompletedTask;
        }

        public Task PublishAsync<T>(T @event, EventProperties properties) where T : class {
            throw new NotImplementedException();
        }

        public Task PublishAsync(string body, EventProperties properties) {
            throw new NotImplementedException();
        }

        public Task ScheduleAsync<T>(T @event, DateTime scheduledEnqueueTimeUtc) where T : class {
            throw new NotImplementedException();
        }

        public Task ScheduleAsync<T>(T @event, DateTime scheduledEnqueueTimeUtc, string correlationId) where T : class {
            throw new NotImplementedException();
        }

        public Task ScheduleAsync<T>(T @event, DateTime scheduledEnqueueTimeUtc, EventProperties properties) where T : class {
            throw new NotImplementedException();
        }

        public Task ScheduleAsync(string body, DateTime scheduledEnqueueTimeUtc, EventProperties properties) {
            throw new NotImplementedException();
        }
    }
}
