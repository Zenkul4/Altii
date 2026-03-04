using Alti.Domain.Entities;

namespace Alti.Domain.Services.Interfaces;

public interface IPaymentDomainService
{
    void Approve(Payment payment, string externalReference);
    void Reject(Payment payment);
    void Refund(Payment payment);
}