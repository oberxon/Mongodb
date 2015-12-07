using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using System.Net.Http;
using System.Net;
using Manage.Service.API.Models;

namespace Manage.Service.API
{
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = new Dictionary<string, IEnumerable<string>>();
                foreach (var kvp in actionContext.ModelState)
                {
                    errors[kvp.Key] = kvp.Value.Errors.Select(e => e.ErrorMessage);
                }
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, new ExecuteResult<KeyValuePair<string, IEnumerable<string>>>
                {
                    Details = errors,
                    ErrorCode = -100
                });
            }
        }
    }
}