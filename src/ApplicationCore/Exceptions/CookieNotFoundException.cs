using ApplicationCore.Exceptions;

namespace ServiceLayer.Exceptions
{
    public class CookieNotFoundException : BaseException
    {
        public CookieNotFoundException(string message) : base(message) { }
    }
}
