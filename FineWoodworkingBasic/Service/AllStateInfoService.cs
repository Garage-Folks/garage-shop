namespace FineWoodworkingBasic.Service
{
    public class AllStateInfoService
    {
        public Dictionary<string, object> currentStateInfo = new Dictionary<string, object>();

        public AllStateInfoService() { currentStateInfo = new Dictionary<string, object>(); }

        public bool IsLoggedIn()
        {
            if (currentStateInfo["currentUser"]!=null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}
