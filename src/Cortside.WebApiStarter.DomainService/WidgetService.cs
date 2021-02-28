using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cortside.DomainEvent;
using Cortside.WebApiStarter.Data;
using Cortside.WebApiStarter.Dto;
using Microsoft.Extensions.Logging;

namespace Cortside.WebApiStarter.DomainService {
    public class WidgetService : IWidgetService {
        private readonly IDatabaseContext db;
        private readonly IDomainEventPublisher publisher;
        private readonly ILogger<WidgetService> logger;

        public WidgetService(IDatabaseContext db,
            IDomainEventPublisher publisher,
            ILogger<WidgetService> logger) {
            this.db = db;
            this.publisher = publisher;
            this.logger = logger;
        }

        public Task<WidgetDto> CreateWidget(string parameter) {
            throw new NotImplementedException();
        }

        public Task<WidgetDto> DeleteWidget(int widgetId) {
            throw new NotImplementedException();
        }

        public Task<WidgetDto> GetWidget(int widgetId) {
            throw new NotImplementedException();
        }

        public Task<List<WidgetDto>> GetWidgets(List<int> widgetIds) {
            throw new NotImplementedException();
        }

        public Task<WidgetDto> UpdateWidget(int widgetId, string parameter) {
            throw new NotImplementedException();
        }
    }
}
