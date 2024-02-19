using FineWoodworkingBasic.Model;
namespace FineWoodworkingBasic.Service
{
    public class AddLumberService
    {
        public async Task<ResultMessage> AddLumberAsync(string area, string locus)
        {
            return await Task.FromResult(AddLumberAsyncHelper(area, locus));
        }

        private ResultMessage AddLumberAsyncHelper(string area, string locus)
        {
            Location location = new Location(area, locus);
            location.Save();
            return location.RetrieveSaveMessage();
        }

    }
}