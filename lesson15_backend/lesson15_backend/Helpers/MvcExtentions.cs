using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace lesson11_backend.Helpers
{
    public static class MvcExtentions
    {
        public static string ActiveClass(this IHtmlHelper htmlHelper,string controller,string action, string ClassName="active")
        {
            var currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;
            var currrentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;

            return (currentController== controller && currrentAction== action)?ClassName:"";
        }
    }
}
