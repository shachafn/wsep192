using ApplicationCore.Interfaces.ExternalServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.ExternalServices
{
    public class SupplyService : BaseHttpClient, ISupplySystem
    {
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

        public bool CancelSupply()
        {
            var postContent = new Dictionary<string, string>
            {
                { "action_type", "cancel_supply" },
                { "transaction_id", "30525" }
            };
            return Int32.Parse(base.Post(postContent)) == 1;
        }
    }
}
