using Microsoft.AspNetCore.Http;
using System;

namespace Utils
{
    public static class HttpContextExtensions
    {
        public static Guid GetSessionID(this HttpContext httpContext) => new Guid(httpContext.Session.Id);
    }
}
