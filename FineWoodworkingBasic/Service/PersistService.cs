using System.Reflection;
using FineWoodworkingBasic.Model;
namespace FineWoodworkingBasic.Service
{
    public class PersistService
    {
        public async Task<ResultMessage> PersistAsync(string model, object[] attributes)
        {
            return await Task.FromResult(PersistAsyncHelper(model, attributes));
        }

        private ResultMessage PersistAsyncHelper(string model, object[] attributes)
        {
            Type? modelType = Type.GetType("FineWoodworkingBasic.Model." + model);
            if (modelType == null) return new ResultMessage(ResultMessage.ResultMessageType.Error, "No model found matching: " + model);

            Type[] attributesTypes = new Type[attributes.Length];

            for (int i = 0; i < attributesTypes.Length; i++)
            {
                attributesTypes[i] = attributes[i].GetType();
            }

            ConstructorInfo? constructor = modelType.GetConstructor(attributesTypes);
            if (constructor == null) return new ResultMessage(ResultMessage.ResultMessageType.Error, "No constructor found matching given attributes");

            dynamic modelItem = constructor.Invoke(attributes);
            modelItem.Save();

            return modelItem.RetrieveSaveMessage();
        }
    }
}
