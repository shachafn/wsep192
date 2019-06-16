using ApplicationCore.Interfaces.ExternalServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Infrastructure.ExternalServices
{
    public class SupplyService : BaseHttpClient, ISupplySystem
    {

        public SupplyService(ILogger<BaseHttpClient> logger) : base(logger)
        {
        }

        public bool IsAvailable()
        {
            var values = new Dictionary<string, string>
            {
               { "action_type", "handshake" }
            };
            return base.Post(values).Equals("OK");
        }

        public Int32 Supply()
        {
            try
            {
                var postContent = new Dictionary<string, string>
                {
                    { "action_type", "supply" },
                    { "name", "Israel Israelovice" },
                    { "address", "Rager Blvd 12" },
                    { "city", "Beer Sheva" },
                    { "country", "Israel" },
                    { "zip", "8458527" }
                };

                return Int32.Parse(base.Post(postContent));
            }
            catch
            {
                return -1;
            }
        }

        public bool CancelSupply()
        {
            try
            {
                var postContent = new Dictionary<string, string>
                {
                    { "action_type", "cancel_supply" },
                    { "transaction_id", "30525" }
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
