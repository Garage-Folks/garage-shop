using FineWoodworkingBasic.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;



namespace FineWoodworkingBasic.Pages.Private
{
    partial class AddBrandPage : ComponentBase
    {
        bool success;
        bool spin;
        AddBrandForm model = new AddBrandForm(); 
        private string addBrandMessage { get; set; }
        private string bName { get; set; }
        private string bNotes { get; set; }

        /// <summary>
        /// Basic form validation passed, checking for existing entries and returning message. 
        /// </summary>
        /// <param name="context"></param>
        private async void OnValidSubmit(EditContext context)
        {
            success = true;

            ResultMessage mesg = await svc.AddBrandAsync(model.Name, model.Notes);
            addBrandMessage = mesg.Message;
            StateHasChanged();
        }

        private IEnumerable<string> MaxFiftyCharacters(string ch)
        { 
            if (!string.IsNullOrEmpty(ch) && 50 < ch?.Length)
                yield return "Max 50 characters";
        }

        private IEnumerable<string> MaxTwoThouCharacters(string ch)
        {
            if (!string.IsNullOrEmpty(ch) && 2000 < ch?.Length)
                yield return "Max 2000 characters";
        }
    }
    public class AddBrandForm
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name length can't be greater than 50 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "Notes length can't be greater than 2000 characters.")]
        public string Notes { get; set; }
    }

}
