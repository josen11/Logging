using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Logging.Web.Pages
{
    public class IndexModel : PageModel
    {
        // private readonly ILogger _logger;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            // The NullLogger is a built-in logger that essentially discards log messages.
            // It is a convenient way to handle cases where a logger is expected, but none is provided or needed.
            // This is recommended when we use logger in Library Classes
            // _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
        }

        public void OnGet()
        {
            _logger.LogInformation("[Jota] Logs from index {request}", HttpContext.Request.Method);
            System.Diagnostics.Trace.TraceError("[Jota] Logs to see in AppServices Logs");
        }
    }
}
