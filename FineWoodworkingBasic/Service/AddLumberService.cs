using FineWoodworkingBasic.Model;
using System.Data.SqlTypes;
namespace FineWoodworkingBasic.Service
{
    public class AddLumberService
    {
        public async Task<ResultMessage> AddLumberAsync(string name, string notes, string fileImg1, string fileImg2, string fileImg3,
            int quantity, double length, double width, double thickness, SqlGuid woodSpeciesId)
        {
            return await Task.FromResult(AddLumberAsyncHelper(name, notes, fileImg1, fileImg2, fileImg3,
            quantity, length, width, thickness, woodSpeciesId));
        }

        private ResultMessage AddLumberAsyncHelper(string name, string notes, string fileImg1, string fileImg2, string fileImg3,
            int quantity, double length, double width, double thickness, SqlGuid woodSpeciesId)
        {
            Lumber lumber = new Lumber(name, notes, fileImg1, fileImg2, fileImg3,
            quantity, length, width, thickness, woodSpeciesId);
            lumber.Save();
            return lumber.RetrieveSaveMessage();
        }
    }
}