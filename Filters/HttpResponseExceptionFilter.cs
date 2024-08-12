using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Data;
using System.Text.Json;
using WebApi.DTOs;
using WebApi.Cache;
using WebApi.Exceptions;

namespace WebApi.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public async void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception is SecureException e)
            {
                var db = context.HttpContext.RequestServices.GetService<StoreContext>();
                var cache = context.HttpContext.RequestServices.GetService<ApiMemoryCache>();
                long nReqId = await cache.GetOrCreate("RequestId");

                var evt = new JournalEvent
                {
                    CreatedAt = DateTime.UtcNow,
                    Text = string.Concat($"Request ID = {nReqId}, ", e.Message, e.StackTrace),
                    Id = nReqId
                };
                db.Events.Add(evt);
                db.SaveChanges();

                ErrorDetails err = new ErrorDetails
                {
                    Data = new DTOs.Data
                    {
                        Message = string.Concat(e.Message, $", ID = {evt.EventId}")
                    },
                    Id = evt.EventId,
                    Type = "Secure"
                };

                context.Result = new ObjectResult(err)
                {
                    StatusCode = 500
                };
                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is Exception ex)
            {
                var db = context.HttpContext.RequestServices.GetService<StoreContext>();
                var cache = context.HttpContext.RequestServices.GetService<ApiMemoryCache>();
                long nReqId = await cache.GetOrCreate("RequestId");

                var evt = new JournalEvent
                {
                    CreatedAt = DateTime.UtcNow,
                    Text = string.Concat($"Request ID = {nReqId}, ", ex.Message, ex.StackTrace),
                    Id = nReqId
                };
                db.Events.Add(evt);
                db.SaveChanges();

                ErrorDetails err = new ErrorDetails
                {
                    Data = new DTOs.Data
                    {
                        Message = string.Concat(ex.Message, $", ID = {evt.EventId}")
                    },
                    Id = evt.Id,
                    Type = typeof(Exception).Name
                };

                context.Result = new ObjectResult(err)
                {
                    StatusCode = 500
                };
                context.ExceptionHandled = true;

                return;
            }
        }
    }

}
