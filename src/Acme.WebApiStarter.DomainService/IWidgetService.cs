using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.WebApiStarter.Dto;

namespace Acme.WebApiStarter.DomainService {
    public interface IWidgetService {
        Task<WidgetDto> CreateWidgetAsync(WidgetDto dto);
        Task<WidgetDto> GetWidgetAsync(int widgetId);
        Task<List<WidgetDto>> GetWidgetsAsync();
        Task<WidgetDto> UpdateWidgetAsync(WidgetDto dto);
        Task<WidgetDto> DeleteWidgetAsync(int widgetId);
        Task PublishWidgetStateChangedEventAsync(int id);
    }
}
