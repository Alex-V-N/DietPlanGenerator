using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.ActionFilters
{
	public class AdminsAuthenticationFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (string.IsNullOrEmpty(context.HttpContext.Session.GetString("Id")))
			{
				context.Result = new RedirectResult("/Authentication/Login");
			}
		}
	}
}
