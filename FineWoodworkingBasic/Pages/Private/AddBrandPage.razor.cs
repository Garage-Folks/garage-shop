using FineWoodworkingBasic.Model;
using Microsoft.AspNetCore.Components;

namespace FineWoodworkingBasic.Pages.Private
{
    partial class AddBrandPage : ComponentBase
    {
        private string addBrandMessage { get; set; }
        private string bName { get; set; }
        private string bNotes { get; set; }

        public string CssClass { get; set; }

        private void SetBName(ChangeEventArgs changeEvent)
        {
            bName = (string)changeEvent.Value;
        }

        private void SetBNotes(ChangeEventArgs changeEvent)
        {
            bNotes = (string)changeEvent.Value;
        }

        private async void AddBrandAction()
        {


            ResultMessage mesg = await svc.AddBrandAsync(bName, bNotes);
            addBrandMessage = mesg.Message;
        }
    }
}
