using Microsoft.EntityFrameworkCore;
using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Infrastructure;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;
using ResoPayment.Service.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace ResoPayment.Service.Implements
{
    public class PaymentProviderService : BaseService<PaymentProviderService>, IPaymentProviderService
    {
        public PaymentProviderService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<PaymentProviderService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, httpContextAccessor, configuration)
        {
        }

        public async Task<IEnumerable<PaymentProviderResponse>> GetAllPaymentTypesByBrandId()
        {
            Guid storeId = Guid.Parse(GetStoreIdFromJwt());
            Store store = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId));
            if (store == null) throw new BadHttpRequestException("Không tìm thấy cửa hàng");
            Guid brandId = store.BrandId;
            var listPaymnetProviderId = await _unitOfWork.GetRepository<BrandPaymentProviderMapping>().GetListAsync(
                    selector: x => x.PaymentProviderId,
                    predicate: x => x.BrandId.Equals(brandId)
                    );
            IEnumerable<PaymentProviderResponse> paymentProviderResponses =
                await _unitOfWork.GetRepository<PaymentProvider>().GetListAsync(
                    selector: x => new PaymentProviderResponse(x.Id, x.Name, x.Type, x.PicUrl),
                    predicate: x => listPaymnetProviderId.Contains(x.Id)
                    );
            return paymentProviderResponses;
        }


    }
}
