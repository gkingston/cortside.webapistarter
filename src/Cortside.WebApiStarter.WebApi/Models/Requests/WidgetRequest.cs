using System;

namespace Cortside.WebApiStarter.WebApi.Models.Requests {

    /// <summary>
    /// Represents a single loan
    /// </summary>
    public class WidgetRequest {
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
