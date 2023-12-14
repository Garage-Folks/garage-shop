using FineWoodworkingBasic.Model;

namespace FineWoodworkingBasic.Service
{
    public class AddBrandService
    {
        public async Task<ResultMessage> AddBrandAsync(string bName, string bNotes)
        {
            return await Task.FromResult(AddBrandAsyncHelper(bName, bNotes));
        }

        private ResultMessage AddBrandAsyncHelper(string bName, string bNotes)
        {
            Brand brand = new Brand(bName, bNotes);
            brand.Save();
            return brand.RetrieveSaveMessage();
        }
    }
}

﻿