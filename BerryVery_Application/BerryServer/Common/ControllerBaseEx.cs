using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Common
{
    public class ControllerBaseEx<Controller, Service> : ControllerBase
    {
        protected readonly ILogger<Controller> _logger;
        protected readonly Service _service;

        public ControllerBaseEx(ILogger<Controller> logger, Service service)
        {
            this._logger = logger;
            this._service = service;
        }
    }
}
