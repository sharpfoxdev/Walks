using System.Net;

namespace WalksAPI.Middlewares {
    public class ExceptionHandlerMiddleware {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, 
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext) {
            try {
                await next(httpContext);
            }
            catch (Exception ex) {
                var errorId = Guid.NewGuid();
                // logging exception
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // return custom error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                // custom error object
                var error = new {
                    Id = errorId,
                    ErrorMessage = "Something went wrong. "
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
