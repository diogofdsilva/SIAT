﻿using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SIAT.WebApplication.Helpers
{
    public static class IsCurrentActionHelper
    {

        public static MvcHtmlString MenuLink(
                            this HtmlHelper htmlHelper,
                            string linkText,
                            string actionName,
                            string controllerName
                        )
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            if (actionName == currentAction && controllerName == currentController)
            {
                return htmlHelper.ActionLink(
                    linkText,
                    actionName,
                    controllerName,
                    null,
                    new
                    {
                        @class = "current_page_item"
                    });
            }

            
            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }
    }
}
