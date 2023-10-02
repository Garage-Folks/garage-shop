using Azure.Identity;
using FineWoodworkingBasic.Model;
namespace FineWoodworkingBasic.Service

{
    public class LoginService
    {
        public Task<ResultMessage> LoginAsync(Dictionary<string, object> stateInfo, string username, string password)
        {
            return Task.FromResult(LoginAsyncHelper(stateInfo, username, password));   
        }

        public ResultMessage LoginAsyncHelper(Dictionary<string, object> stateInfo, string uname, string pwd)
        {
            string loginMessage;
            FineWoodworkingBasic.Model.AuthorizedUser au = new FineWoodworkingBasic.Model.AuthorizedUser();

            au.Populate(uname);

            if (au.IsPopulated())
            {
                if (au.CheckIfPasswordsMatch(pwd))
                {
                    loginMessage = "Login successful!";
                    stateInfo["userName"] = uname;
                    return new ResultMessage(ResultMessage.ResultMessageType.Success, loginMessage);
                }
                else
                {
                    loginMessage = "ERROR: Passwords don't match!";
                    stateInfo.Remove("userName");
                    return new ResultMessage(ResultMessage.ResultMessageType.Error, loginMessage);
                }
            }
            else
            {
                ResultMessage mesg = au.RetrievePopulateMessage();
                if (mesg != null)
                {
                    loginMessage = "ERROR: User with user name: " + uname + " not found!";
                    
                }
                else
                {
                    loginMessage = "ERROR: Unexpected error in retrieving user with user name: " + uname + " from database!";
                    
                }
                stateInfo.Remove("userName");
                return new ResultMessage(ResultMessage.ResultMessageType.Error, loginMessage);

            }

        }

    
    }
}
