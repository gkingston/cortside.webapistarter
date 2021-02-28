using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cortside.DomainEvent;
using Cortside.DomainEvent.Events;
using Cortside.WebApiStarter.Data;
using Cortside.WebApiStarter.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cortside.WebApiStarter.DomainService {
    public class WidgetService : IWidgetService {
        private readonly DatabaseContext db;
        private readonly IDomainEventOutboxPublisher publisher;
        private readonly ILogger<WidgetService> logger;

        public WidgetService(DatabaseContext db, IDomainEventOutboxPublisher publisher, ILogger<WidgetService> logger) {
            this.db = db;
            this.publisher = publisher;
            this.logger = logger;
        }

        public async Task<WidgetDto> CreateWidget(WidgetDto dto) {
            var entity = new Domain.Widget() {
                Text = dto.Text,
                Width = dto.Width,
                Height = dto.Height
            };

            db.WebApiStarter.Add(entity);
            var @event = new WidgetStageChangedEvent() { Text = entity.Text };
            await publisher.SendAsync(@event);
            await db.SaveChangesAsync();

            return ToWidgetDto(entity);
        }

        public async Task<WidgetDto> DeleteWidget(int widgetId) {
            throw new NotImplementedException();
        }

        public async Task<WidgetDto> GetWidget(int widgetId) {
            var entity = db.WebApiStarter.Single(x => x.WidgetId == widgetId);
            return ToWidgetDto(entity);
        }

        public async Task<List<WidgetDto>> GetWidgets() {
            var entities = await db.WebApiStarter.ToListAsync();

            var dtos = new List<WidgetDto>();
            foreach (var entity in entities) {
                dtos.Add(ToWidgetDto(entity));
            }

            return dtos;
        }

        public async Task<WidgetDto> UpdateWidget(WidgetDto dto) {
            throw new NotImplementedException();
        }

        private WidgetDto ToWidgetDto(Domain.Widget entity) {
            return new WidgetDto() {
                WidgetId = entity.WidgetId,
                Text = entity.Text,
                Width = entity.Width,
                Height = entity.Height
            };
        }
    }
}
