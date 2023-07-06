namespace RealEstateApi.Contracts.Data
{
    public class UploadImageDto
    {
        public IFormFile FormData { get; set; }
        public int IdAdvertisement { get; set; }
    }
}
