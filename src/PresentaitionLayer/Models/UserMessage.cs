using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentaitionLayer.Models
{
    public class UserMessage
    {
        public UserMessage(string redirect, string message,string image)
        {
            Redirect = redirect;
            Message = message;
            Image = image;
        }
        public UserMessage(string redirect, string message)
        {
            Redirect = redirect;
            Message = message;
        }
        public string Image { get; set; }
        public string Redirect { get; set; }
        public string Message { get; set; }
    }
}
