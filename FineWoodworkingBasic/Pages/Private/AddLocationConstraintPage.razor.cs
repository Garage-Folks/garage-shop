using FineWoodworkingBasic.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using MudBlazor;

namespace FineWoodworkingBasic.Pages.Private
{
    #region AddLocationConstraintPage Class
    partial class AddLocationConstraintPage : ComponentBase
    {
        // Parameter with helper variables to maintain a modified and unmodified state of the form.
        public AddLocationConstraintForm model = new AddLocationConstraintForm();
        private string addLocationConstraintMessage { get; set; }

        Color msgColor = Color.Error;
        bool spin;

        /// <summary>
        /// Basic form validation passed, checking for existing entries and returning message. 
        /// </summary>
        /// <param name="context"></param>
        private async void OnValidSubmit(EditContext context)
        {
            spin = true;

            ResultMessage mesg = await svc.AddLocationConstraintAsync(model.Description);

            addLocationConstraintMessage = mesg.Message;
            string _compare = "success";
            if (addLocationConstraintMessage.Contains(_compare))
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
            model.Description = string.Empty;
            addLocationConstraintMessage = string.Empty;
            StateHasChanged();
        }

        #region Error Messages (Validation Tag)

        private IEnumerable<string> MaxTwoThouCharacters(string ch)
        {
            if (!string.IsNullOrEmpty(ch) && 2000 < ch?.Length)
                yield return "Max 25 characters";
        }
        #endregion

    }
    #endregion

    #region AddLocationConstraintForm Class
    public class AddLocationConstraintForm
    {
        [Required(ErrorMessage = "Description field must have a value.")]
        [StringLength(2000, ErrorMessage = "Description length can't be more than 2000.")]
        public string Description { get; set; }
    }
}
    #endregion
