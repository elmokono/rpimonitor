using homeMonitor.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace homeMonitor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly MonitorService MonitorService;

        public IndexModel(ILogger<IndexModel> logger, MonitorService service)
        {
            _logger = logger;
            MonitorService = service;
        }

        public void OnGet()
        {

        }
    }
}
