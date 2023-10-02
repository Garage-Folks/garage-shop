using FineWoodworkingBasic.Model;

namespace FineWoodworkingBasic.Service
{
    public class AddBrandService
    {
        public Task<ResultMessage> AddBrandAsync(Dictionary<string, object> stateInfo, string bName, string bNotes)
        {
            return Task.FromResult(AddBrandAsyncHelper(stateInfo, bName, bNotes));
        }

        private ResultMessage AddBrandAsyncHelper(Dictionary<string, object> stateInfo, string bName, string bNotes)
        {

             bool isLoggedIn = stateInfo.ContainsKey("userName");

            if (isLoggedIn == true)
            {
                string loggedInUserId = stateInfo["userName"].ToString();
                FineWoodworkingBasic.Model.Brand brand = new FineWoodworkingBasic.Model.Brand(bName, bNotes);
                brand.Save();
                return brand.RetrieveSaveMessage();
            }
            else
            {
                return new ResultMessage(ResultMessage.ResultMessageType.Error, 
                    "No valid credentials for this operation found!");
            }
            



        }

    }
}

﻿