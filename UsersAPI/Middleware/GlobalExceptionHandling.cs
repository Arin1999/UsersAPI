using System.Net;

namespace UsersAPI.Middleware
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
