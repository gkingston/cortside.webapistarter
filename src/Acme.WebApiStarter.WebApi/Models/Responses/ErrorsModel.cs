using System.Collections.Generic;

namespace Acme.WebApiStarter.WebApi.Models.Responses {
    /// <summary>
    /// Errors Model
    /// </summary>
    public class ErrorsModel {
        /// <summary>
        /// Errors model constructor
        /// </summary>
        public ErrorsModel() {
            Errors = new List<ErrorModel>();
        }

        /// <summary>
        /// Errors List
        /// </summary>
        public List<ErrorModel> Errors { get; set; }

        /// <summary>
        /// Adds error model
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void AddError(string type, string message) {
            var error = new ErrorModel() {
                Type = type,
                Message = message
            };
            Errors.Add(error);
        }
    }
}
