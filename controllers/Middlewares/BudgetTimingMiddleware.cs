using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BudgetP
{
    public class BudgetTimingFilter : IActionFilter
    {
        private readonly ILogger<BudgetTimingFilter> _logger;

        public BudgetTimingFilter(ILogger<BudgetTimingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("BudgetTimingFilter reached...");

            var today = DateTime.UtcNow.Date;

            bool isOpen = Utility.IsBudgetOpen();
            _logger.LogInformation("Budget status: {Status}", isOpen);

            if (!isOpen)
            {
                _logger.LogWarning("No active budget found for today ({Today})", today);

                // 🚫 Short-circuit: prevent action execution
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = "No open budget for today."
                };

                return;
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // optional post-action logic
        }
    }
}
