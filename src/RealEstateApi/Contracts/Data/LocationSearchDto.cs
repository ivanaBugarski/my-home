namespace RealEstateApi.Contracts.Data
{
    public class LocationSearchDto
    {
        public int Radius { get; set; }
        public PositionDto Position { get; set; }
    }
}
