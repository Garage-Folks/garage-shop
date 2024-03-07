using FineWoodworkingBasic.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using MudBlazor;
using System.Collections;

namespace FineWoodworkingBasic.Pages.Private
{
    #region AddLocationPage Class
    partial class AddLocationPage : ComponentBase
    {
        // Parameter with helper variables to maintain a modified and unmodified state of the form.
        public AddLocationForm model = new AddLocationForm();
        private string addLocationMessage { get; set; }
        Location location = new Location();
        List<Location> AreaList = new List<Location>();

        Color msgColor = Color.Error;
        bool spin;

        /// <summary>
        /// Basic form validation passed, checking for existing entries and returning message. 
        /// </summary>
        /// <param name="context"></param>
        private async void OnValidSubmit(EditContext context)
        {
            spin = true;

            ResultMessage mesg = await svc.AddLocationAsync(model.Area, model.Locus);

            addLocationMessage = mesg.Message;
            string _compare = "success";
            if (addLocationMessage.Contains(_compare))
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
            model.Area = string.Empty;
            model.Locus = string.Empty;
            addLocationMessage = string.Empty;
            StateHasChanged();
        }

        #region Error Messages (Validation Tag)
        private IEnumerable<string> MaxTenCharacters(string ch)
        {
            if (!string.IsNullOrEmpty(ch) && 10 < ch?.Length)
                yield return "Max 10 characters";
        }

        private IEnumerable<string> MaxTwentyFiveCharacters(string ch)
        {
            if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
                yield return "Max 25 characters";
        }
        #endregion
        private async Task<IEnumerable<string>> Search1(string value)
        {
            await OnInitializedAsync();
            return AreaList.Select(x => x.Area).Distinct().ToArray();
        }
        protected override Task OnInitializedAsync()
        {
            AreaList = PopulateAreaList();
            return base.OnInitializedAsync();
        }
        private List<Location> PopulateAreaList()
        {
            LocationCollection allLocationCollection = new LocationCollection();
            allLocationCollection.PopulateAll();
            return allLocationCollection.LocationList;
        }

        #endregion

        #region AddLocationForm Class
        public class AddLocationForm
        {
            [Required(ErrorMessage = "The Area field must have a value.")]
            [StringLength(10, ErrorMessage = "Area length can't be more than 10.")]
            public string Area { get; set; }
            [Required(ErrorMessage = "The Locus field must have a value.")]
            [StringLength(25, ErrorMessage = "Locus length can't be more than 25.")]
            public string Locus { get; set; }
        }

    }
    #endregion
}