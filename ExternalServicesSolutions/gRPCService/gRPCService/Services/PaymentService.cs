using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Payment;

namespace gRPCService
{
    public class PaymentService : Payment.Payer.PayerBase
    {
        public override Task<IsAvailableReply> IsAvailable(IsAvailableRequest request, ServerCallContext context)
        {
            return Task.FromResult(new IsAvailableReply
            {
                Result = true
            });
        }

        public override Task<GetUserBalanaceReply> GetUserBalanace(GetUserBalanaceRequest request, ServerCallContext context)
        {
            return Task.FromResult<GetUserBalanaceReply>(new GetUserBalanaceReply
                {
                    Balance = new Random().NextDouble()*200
                }
            );
        }

        public override Task<PayForUserReply> PayForUser(PayForUserRequest request, ServerCallContext context)
        {
            return Task.FromResult<PayForUserReply>(new PayForUserReply
            {
                IsSuccess = true
            });
        }
    }
}
