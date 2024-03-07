using FineWoodworkingBasic.Model;
namespace FineWoodworkingBasic.Service
{
    public class AddLocationConstraintService
    {
        public async Task<ResultMessage> AddLocationConstraintAsync(string description)
        {
            return await Task.FromResult(AddLocationConstraintAsyncHelper(description));
        }

        private ResultMessage AddLocationConstraintAsyncHelper(string description)
        {
            LocationConstraint lc = new LocationConstraint(description);
            lc.Save();
            return lc.RetrieveSaveMessage();
        }

    }
}