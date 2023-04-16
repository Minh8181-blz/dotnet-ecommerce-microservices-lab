using API.Payment.Domain.Entities;
using Domain.Base.SeedWork;
using System;
using System.Threading.Tasks;

namespace API.Payment.Domain.Interfaces
{
    public interface IPaymentOperationRepository : IRepository<PaymentOperation, Guid>
    {
        Task<PaymentOperation> GetPaymentOperationByOrderIdAsync(int orderId);
    }
}
