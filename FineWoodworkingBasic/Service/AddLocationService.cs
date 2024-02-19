using FineWoodworkingBasic.Model;
namespace FineWoodworkingBasic.Service
{
    public class AddLocationService
    {
        public async Task<ResultMessage> AddLocationAsync(string area, string locus)
        {
            return await Task.FromResult(AddLocationAsyncHelper(area, locus));
        }

        private ResultMessage AddLocationAsyncHelper(string area, string locus)
        {
            Location location = new Location(area, locus);
            location.Save();
            return location.RetrieveSaveMessage();
        }

    }
}