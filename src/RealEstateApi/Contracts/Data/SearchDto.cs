using Newtonsoft.Json.Linq;
using RealEstateApi.Model.Enum;

namespace RealEstateApi.Contracts.Data
{
    public record SearchDto(RealEstateType RealEstateType, AdvertisementType AdvertisementType, string City, int Price, int Quadrature);
}
