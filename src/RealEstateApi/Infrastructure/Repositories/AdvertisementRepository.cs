using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApi.Contracts.Data;
using RealEstateApi.Contracts.Repositories;
using RealEstateApi.Model;

namespace RealEstateApi.Infrastructure.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly CoreContext _context;

        public AdvertisementRepository(CoreContext context)
        {
            _context = context;
        }

        public async Task<Advertisement> AddAdvertisementAsync(Advertisement advertisement)
        {
            Advertisement advertisementSaved = (await _context.Advertisements.AddAsync(advertisement)).Entity;
            await _context.SaveChangesAsync();

            return advertisementSaved;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertisementsAsync()
        {
            IEnumerable<Advertisement> advertisements = await _context.Advertisements.Where(x => x.IsPublished).Include(x => x.Address).Include(x => x.Images).ToListAsync();
            return advertisements;
        }
        
        public async Task<IEnumerable<Advertisement>> GetAllAdvertisementBySearchAsync(AdvertisementDto search)
        {
            return await Searching(search);
        }

        public async Task<Advertisement> GetAdvertisementByIdAsync(int id)
        {
            Advertisement advertisement = await _context.Advertisements.Where(x => x.IsPublished).Include(x => x.Address).Include(x => x.Images).SingleOrDefaultAsync(x => x.Id == id);
            if (advertisement == null)
            {
                throw new NotImplementedException();
            }
            return advertisement;
        }

        public async Task<IEnumerable<Image>> GetImageByAdvertisement(int idAdv)
        {
            return await _context.Images.Where(x => x.AdvertisementId == idAdv).ToListAsync();
        }

        public async Task<IEnumerable<Advertisement>> GetChosenAdvertisementsAsync(string id)
        {
            return _context.Advertisements
                .Where(x => x.UserId == id && x.IsFavourite == true && x.IsPublished && !x.IsDeleted).Include(x => x.Address).Include(x => x.Images).AsEnumerable();
        }

        public async Task<IEnumerable<Advertisement>> GetMyAdvertisementsAsync(string id)
        {
            return _context.Advertisements.Where(x => x.User.Id == id && x.IsPublished).Include(x => x.Address).Include(x => x.Images).AsEnumerable();
        }

        public async Task<Advertisement> GetLast()
        {
            return await _context.Advertisements.OrderBy(x => x.Id).LastOrDefaultAsync();
        }

        public async Task BuyPointsAsync(int points, string id)
        {
            Advertisement advertisement = _context.Advertisements.Where(x => x.User.Id == id && !x.IsDeleted && x.IsPublished).SingleOrDefault();
            if (advertisement != null)
            {
                advertisement.User.Points = advertisement.User.Points + points;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IResult> ChooseAdvertisementAsync(IdDto ids)
        {
            Advertisement advertisement = await _context.Advertisements
                .Where(x => x.UserId == ids.IdUser && x.Id == ids.Id && !x.IsDeleted).SingleOrDefaultAsync();

            advertisement.IsFavourite = true;
 
            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IEnumerable<Advertisement>> Searching(AdvertisementDto advertisementDto)
        {
            List<Advertisement> advertisement = (List<Advertisement>)_context.Advertisements
                .Where(x => x.Price == advertisementDto.Price
            || x.AdvertisementType == advertisementDto.AdvertisementType
            || x.Type == advertisementDto.RealEstateType
            || x.Quadrature == advertisementDto.Quadrature
            || x.Address.City == advertisementDto.City);

            return advertisement;
        }

        public async Task<IEnumerable<Advertisement>> FastSearch(FastSearchDto fastSearchDto)
        {
            return await _context.Advertisements
                .Where(x => x.Address.City.Replace(" ", string.Empty).Equals(fastSearchDto.CityType.ToString())
                && x.Type == fastSearchDto.RealEstateType
                && !x.IsDeleted).Include(x => x.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<Advertisement>> LocationSearch(LocationSearchDto locationSearchDto)
        {
            return await _context.Advertisements
                .Where(x => x.Address.Lat < locationSearchDto.Radius && x.Address.Lng < locationSearchDto.Radius && !x.IsDeleted).Include(x => x.Address).ToListAsync();
        }

        public async Task<IEnumerable<Advertisement>> GetSearchedAdvertisementsAsync(SearchDto s)
        {
            var list = await _context.Advertisements
                .Where(x => (x.Address.City.Equals(s.City) || x.Address.City.Equals("Belgrade")
                    || x.Quadrature == s.Quadrature
                    || x.Price == s.Price || x.AdvertisementType == s.AdvertisementType
                    || x.Type == s.RealEstateType) && x.IsPublished && !x.IsDeleted).Include(x => x.Address)
                    .Include(x => x.Images).ToListAsync ();
            return list;
        }

        public async Task<Advertisement> GetIdAdvertisementAsync(int idAdvertisement)
        {
            Advertisement adv = await _context.Advertisements.Where(x => x.Id == idAdvertisement && x.IsPublished).Include(x => x.Address).SingleOrDefaultAsync();
            return adv;
        }

        public async Task<IResult> UploadImage(int realEstateId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("No file uploaded");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine("wwwroot", "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            await _context.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
