using FineWoodworkingBasic.Model;
namespace FineWoodworkingBasic.Service
{
    public class AddLocationConstraintService
    {
        public Task<String> AddLocationConstraintAsync(string description)
        {
            return Task.FromResult(AddLocationConstraintAsyncHelper(description));
        }

        private String AddLocationConstraintAsyncHelper(string description)
        {
            LocationConstraint lc = new LocationConstraint(description);
            lc.Save();
            ResultMessage msg = lc.RetrieveSaveMessage();
            String retVal = msg.Message;
            return retVal;
        }

    }
}