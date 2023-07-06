using MediatR;
using RealEstateApi.Contracts.Services;
using RealEstateApi.Model;

namespace RealEstateApi.Features.AdvertisementFeatures
{
    public class GetImageById
    {
        public record Request(int id) : IRequest<IEnumerable<Image>>;

        public class Handler : IRequestHandler<Request, IEnumerable<Image>>
        {
            private readonly IAdvertisementService advertisementService;
            public Handler(IAdvertisementService advertisementService)
            {
                this.advertisementService = advertisementService;
            }
            public async Task<IEnumerable<Image>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await advertisementService.GetImageByAdvertisement(request.id);
            }
        }
    }
}
