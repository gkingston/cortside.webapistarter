using System;
namespace Cortside.WebApiStarter.Dto {

    public class WidgetDto : AuditableEntityDto {

        public Guid WebApiStarterId { get; set; }
    }
}
