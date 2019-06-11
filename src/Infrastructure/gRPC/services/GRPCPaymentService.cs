using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using ApplicationCore.Interfaces.ExternalServices;
using Grpc.Core;
using Payment;
using Channel = Grpc.Core.Channel;

namespace Infrastructure.gRPC.services
{
    public class GRPCPaymentService : Payment.Payer.PayerClient//, IPaymentSystem
    {
        private Channel _channel;
        private Payer.PayerClient _client;

        public GRPCPaymentService()
        {
            _channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            _client = new Payment.Payer.PayerClient(_channel);
        }

        
        public bool IsAvailable()
        {
            var request = new IsAvailableRequest();
            try
            {
                return _client.IsAvailable(request).Result;
            }
            catch(RpcException)
            {
                return false;
            }
        }

        public double GetBalance(Guid userGuid)
        {
            var request = new GetUserBalanaceRequest { UserGuid = userGuid.ToString() };
            return _client.GetUserBalanace(request).Balance;
        }

        public bool PayForUser(Guid userGuid, double amount)
        {
            var request = new PayForUserRequest { UserGuid = userGuid.ToString(), Amount = amount };
            return _client.PayForUser(request).IsSuccess;
        }
    }
}
