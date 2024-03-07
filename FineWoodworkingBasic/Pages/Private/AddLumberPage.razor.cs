using FineWoodworkingBasic.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using MudBlazor;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc;

namespace FineWoodworkingBasic.Pages.Private
{
    #region AddLocationPage Class
    partial class AddLumberPage : ComponentBase
    {
        // Parameter with helper variables to maintain a modified and unmodified state of the form.
        public AddLumberForm model = new AddLumberForm();
        private string addLumberMessage { get; set; }
        List<WoodSpecies> SpeciesList = new List<WoodSpecies>();
        List<Location> AreaList = new List<Location>();
        private int woodSpeciesCollectionSize { get; set; }
        private int areaCollectionSize { get; set; }
        private SqlGuid woodSpeciesId;

        Color msgColor = Color.Error;
        bool spin;

        /// <summary>
        /// Basic form validation passed, checking for existing entries and returning message. 
        /// </summary>
        /// <param name="context"></param>
        private async void OnValidSubmit(EditContext context)
        {
            spin = true;

            for (int i = 0; i < SpeciesList.Count; i++)
            {
                WoodSpecies woodSpecies = SpeciesList[i];
                if (woodSpecies.Name.Equals(model.WoodSpeciesName))
                {
                    woodSpeciesId = woodSpecies.ID;
                    break;
                }
            }


                ResultMessage mesg = await svc.AddLumberAsync(model.Name, model.Notes, model.FileImg1,
                model.FileImg2, model.FileImg3, model.Quantity, model.Length, model.Width, model.Thickness, woodSpeciesId);

            addLumberMessage = mesg.Message;
            string _compare = "success";
            if (addLumberMessage.Contains(_compare))
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
            model.Quantity = 1;
            model.Length = 0;
            model.Width = 0;
            model.Thickness = 0;
            model.WoodSpeciesName = string.Empty;
            model.FileImg1 = string.Empty;
            model.FileImg2 = string.Empty;
            model.FileImg3 = string.Empty;

            addLumberMessage = string.Empty;
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
        /*private async Task<IEnumerable<string>> Search1(string value)
        {
            await OnInitializedAsync();
            return AreaList.Select(x => x.Area).Distinct().ToArray();
        }*/
        protected override Task OnInitializedAsync()
        {

            model.Quantity = 1;
            SpeciesList = PopulateSpeciesList();
            AreaList = PopulateAreaList();
            woodSpeciesCollectionSize = SpeciesList.Count();
            return base.OnInitializedAsync();
        }
    
        private List<WoodSpecies> PopulateSpeciesList()
        {
            WoodSpeciesCollection AllWoodSpecies = new WoodSpeciesCollection();
            AllWoodSpecies.PopulateAll();
            return AllWoodSpecies.WoodSpeciesList;
        }
        private List<Location> PopulateAreaList()
        {
            LocationCollection allLocationCollection = new LocationCollection();
            allLocationCollection.PopulateAll();
            return allLocationCollection.LocationList;
        }

        #endregion

        #region AddLocationForm Class
        public class AddLumberForm
        {
            [Required(ErrorMessage = "The Name field must have a value.")]
            [StringLength(50, ErrorMessage = "Name length can't be more than 50.")]
            public string Name { get; set; }
            [Required(ErrorMessage = "The Notes field must have a value.")]
            [StringLength(2000, ErrorMessage = "Notes length can't be more than 2000.")]
            public string Notes { get; set; }
            [Required(ErrorMessage = "The Quantity field must have a value.")]
            public int Quantity { get; set; }
            [Required(ErrorMessage = "The Length field must have a value.")]
            public double Length { get; set; }
            [Required(ErrorMessage = "The Width field must have a value.")]
            public double Width { get; set; }
            [Required(ErrorMessage = "The Thinkness field must have a value.")]
            public double Thickness { get; set; }
            [Required(ErrorMessage = "The Wood Species field must have a value.")]
            public string WoodSpeciesName { get; set; }
            public string FileImg1 { get; set; }
            public string FileImg2 { get; set; }
            public string FileImg3 { get; set; }


            [StringLength(10, ErrorMessage = "Area length can't be more than 10.")]
            public string Area { get; set; }
            [StringLength(25, ErrorMessage = "Locus length can't be more than 25.")]
            public string Locus { get; set; }
        }
        private async Task<IEnumerable<string>> SearchLumber(string value)
        {
            await OnInitializedAsync();
            return SpeciesList.Select(x => x.Name);
        }

        private async Task<IEnumerable<string>> SearchArea(string value)
        {
            await OnInitializedAsync();
            string tempArea = model.Area;
            return AreaList.Select(x => x.Area).Distinct().ToArray();
        }
        private async Task<IEnumerable<string>> SearchLocus(string value)
        {
            await OnInitializedAsync();
            return AreaList.Select(x => x.Locus).Distinct().ToArray();
        }

    }
    #endregion
}