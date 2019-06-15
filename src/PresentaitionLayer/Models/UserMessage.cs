using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentaitionLayer.Models
{
    public class UserMessage
    {
        public UserMessage(string redirect, string message)
        {
            this.redirect = redirect;
            Message = message;
        }

        public string redirect { get; set; }
        public string Message { get; set; }
    }
}
