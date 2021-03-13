using System.Collections.Generic;
using System.Threading.Tasks;
using Cortside.WebApiStarter.Dto;

namespace Cortside.WebApiStarter.DomainService {
    public interface IWidgetService {
        Task<WidgetDto> CreateWidget(WidgetDto dto);
        Task<WidgetDto> GetWidget(int widgetId);
        Task<List<WidgetDto>> GetWidgets();
        Task<WidgetDto> UpdateWidget(WidgetDto dto);
        Task<WidgetDto> DeleteWidget(int widgetId);
    }
}
