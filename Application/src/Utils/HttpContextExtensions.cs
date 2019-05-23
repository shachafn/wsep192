using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public static class HttpContextExtensions
    {
        public static Guid GetSessionID(this HttpContext httpContext) => new Guid(httpContext.Session.Id);
    }
}
