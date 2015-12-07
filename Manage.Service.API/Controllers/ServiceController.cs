using Manage.Service.API.Models;
using Manage.Service.MongoDb;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Text;
using Manage.Service.MongoDb.Behaviors.Entities;
using System.Configuration;

namespace Manage.Service.API.Controllers
{
    [ModelValidationFilter]
    public class ServiceController : ApiController
    {
        public async Task<ExecuteResult<KeyValuePair<string, object>>> PostAsync(ExecuteCommand command)
        {
            var inputKvp = command.GetCommandArgs();

            try
            {
                return new ExecuteResult<KeyValuePair<string, object>> { Details = await RuleSettings.Rules.RunAsync(command.Name, new CommandContext(inputKvp)) };
            }
            catch (AggregateException ex)
            {
                bool isArgumentExp = false;
                string errorMsg = null;

                ex.Flatten().Handle(innerExp =>
                {
                    if (innerExp is ArgumentException)
                    {
                        isArgumentExp = true;
                        errorMsg = innerExp.Message;
                    }
                    else
                    {
                        if (!isArgumentExp)
                            errorMsg = innerExp.ToString();
                    }
                    return true;
                });

                return new ExecuteResult<KeyValuePair<string, object>>
                {
                    ErrorCode = isArgumentExp ? -1 : -3,
                    ErrorMsg = errorMsg,
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ExecuteResult<string> Get(string name)
        {
            return ReturnWarpper(() => new ExecuteResult<string>
            {
                Details = RuleSettings.Rules.GetCommandRequiredKeys(name)
            });
        }

        private static ExecuteResult<T> ReturnWarpper<T>(Func<ExecuteResult<T>> runner)
        {
            try
            {
                return runner();
            }
            catch (ArgumentException ex)
            {
                return new ExecuteResult<T>
                {
                    ErrorCode = -1,
                    ErrorMsg = ex.Message
                };
            }
            catch (AggregateException ex)
            {
                var sb = new StringBuilder();
                ex.Flatten().Handle(innerExp =>
                {
                    sb.AppendLine(innerExp.ToString());
                    return true;
                });
                return new ExecuteResult<T>
                {
                    ErrorCode = -2,
                    ErrorMsg = sb.ToString()
                };
            }
            catch (Exception ex)
            {
                return new ExecuteResult<T>
                {
                    ErrorCode = -3,
                    ErrorMsg = ex.ToString()
                };
            }
        }

    }
}
