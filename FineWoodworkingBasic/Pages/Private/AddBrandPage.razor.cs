using FineWoodworkingBasic.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using MudBlazor;

namespace FineWoodworkingBasic.Pages.Private
{
    #region AddBrandPage Class
    partial class AddBrandPage : ComponentBase
    {
        // Parameter with helper variables to maintain a modified and unmodified state of the form.
        public AddBrandForm model = new AddBrandForm();
        private string addBrandMessage { get; set; }

        Color msgColor = Color.Error;
        bool spin;

        /// <summary>
        /// Basic form validation passed, checking for existing entries and returning message. 
        /// </summary>
        /// <param name="context"></param>
        private async void OnValidSubmit(EditContext context)
        {
            spin = true;

            ResultMessage mesg = await svc.AddBrandAsync(model.Name, model.Notes);

            addBrandMessage = mesg.Message;
            string _compare = "success";
            if (addBrandMessage.Contains(_compare))
            {
                msgColor = Color.Success;
            }

            spin = false;
            StateHasChanged();
        }

        /// <summary>
        /// Returns all fields to empty and resets the status message.
        /// </summary>
        private void ClearAll()
        {
            model.Name = string.Empty;
            model.Notes = string.Empty;
            addBrandMessage = string.Empty;
            StateHasChanged();
        }

        #region Error Messages (Validation Tag)
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
        #endregion

    }
    #endregion

    #region AddBrandForm Class
    public class AddBrandForm
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name length can't be greater than 50 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "Notes length can't be greater than 2000 characters.")]
        public string Notes { get; set; }
    }
    #endregion
}
