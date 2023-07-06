using MediatR;
using RealEstateApi.Contracts.Data;
using RealEstateApi.Contracts.Services;
using RealEstateApi.Model;

namespace RealEstateApi.Features.AdvertisementFeatures
{
    public class UploadImage
    {
        public record Request(int idAdv, IFormFile file) : IRequest<IResult>;

        public class Handler : IRequestHandler<Request, IResult>
        {
            private readonly IAdvertisementService advertisementService;
            public Handler(IAdvertisementService advertisementService)
            {
                this.advertisementService = advertisementService;
            }
            public async Task<IResult> Handle(Request request, CancellationToken cancellationToken)
            {
                return await advertisementService.UploadImage(request.idAdv, request.file);
            }
        }
    }
}
