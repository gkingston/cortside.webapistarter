using System.Collections.Generic;
using System.Threading.Tasks;
using Cortside.WebApiStarter.Dto;

namespace Cortside.WebApiStarter.DomainService {
    public interface IWidgetService {
        Task<WidgetDto> CreateWidget(string parameter);
        Task<WidgetDto> GetWidget(int widgetId);
        Task<List<WidgetDto>> GetWidgets(List<int> widgetIds);
        Task<WidgetDto> UpdateWidget(int widgetId, string parameter);
        Task<WidgetDto> DeleteWidget(int widgetId);
    }
}
