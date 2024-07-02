using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace priceTracker.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action, string cssClass = "active")
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.Values["action"]?.ToString() ?? string.Empty;
            var currentController = routeData.Values["controller"]?.ToString() ?? string.Empty;

            bool isActive = string.Equals(currentController, controller, System.StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(currentAction, action, System.StringComparison.OrdinalIgnoreCase);

            return isActive ? cssClass : string.Empty;
        }
    }
}
