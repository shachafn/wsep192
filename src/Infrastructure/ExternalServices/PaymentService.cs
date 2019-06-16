using ApplicationCore.Interfaces.ExternalServices;
using System;
using System.Collections.Generic;

namespace Infrastructure.ExternalServices
{
    public class PaymentService : BaseHttpClient, IPaymentSystem
    {
        public bool IsAvailable()
        {
            var values = new Dictionary<string, string>
            {
               { "action_type", "handshake" }
            };
            return base.Post(values).Equals("OK");
        }

        public Int32 Pay()
        {
            try
            {
                var postContent = new Dictionary<string, string>
                {
                    { "action_type", "pay" },
                    { "card_number", "2222333344445555" },
                    { "month", "4" },
                    { "year", "2021" },
                    { "holder", "Israel Israelovice" },
                    { "ccv", "262" },
                    { "id", "20444444" }
                };
                return Int32.Parse(base.Post(postContent));
            }
            catch
            {
                return -1;
            }
        }

        public bool CancelPayment()
        {
            try
            {
                var postContent = new Dictionary<string, string>
                {
                    { "action_type", "cancel_pay" },
                    { "transaction_id", "20123" }
                };
                return Int32.Parse(base.Post(postContent)) == 1;
            }
            catch
            {
                return false;
            }
        }
    }
}
