using System.Reflection;
using FineWoodworkingBasic.Model;
using static MudBlazor.Icons.Custom;
namespace FineWoodworkingBasic.Service
{
    public class AddTemplateService
    {
        private Type ModelType { get; set; }
        public AddTemplateService(string modelType)
        {
            Type? temp = Type.GetType("FineWoodworkingBasic.Model." + modelType) ?? throw new ArgumentException("Unknown model class: " + modelType);
            ModelType = temp;
        }

        public ResultMessage Add(object[] attributes)
        {
            Type[] attributesTypes = new Type[attributes.Length];
            for (int i = 0; i < attributesTypes.Length; i++)
            {
                attributesTypes[i] = attributes[i].GetType();
            }
            ConstructorInfo constructor = ModelType.GetConstructor(attributesTypes) ?? throw new ArgumentException("No constructor found matching attributes.");
            dynamic modelItem = constructor.Invoke(attributes);
            modelItem.Save();
            return modelItem.RetrieveSaveMessage();
        }
    }
}
