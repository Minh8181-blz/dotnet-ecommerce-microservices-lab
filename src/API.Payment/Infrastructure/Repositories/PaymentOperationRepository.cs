using API.Payment.Domain.Entities;
using API.Payment.Domain.Enums;
using API.Payment.Domain.Interfaces;
using Infrastructure.Base.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace API.Payment.Infrastructure.Repositories
{
    public class PaymentOperationRepository : RepositoryBase<PaymentContext, PaymentOperation, Guid>, IPaymentOperationRepository
    {
        public PaymentOperationRepository(PaymentContext paymentContext) : base(paymentContext)
        {
        }

        public async Task<PaymentOperation> GetPaymentOperationByOrderIdAsync(int orderId)
        {
            var payment = await _context.PaymentOperations
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            return payment;
        }
    }
}
