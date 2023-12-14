using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{ 
    public abstract class InventoryItem : DeletablePersistable
    {
        protected SqlGuid? ID { get; set; }
        protected string Name { get; set; }
        protected string Notes { get; set; }
        protected string? FileImage1 { get; set; }
        protected string? FileImage2 { get; set; }
        protected string? FileImage3 { get; set; }
        protected int Quantity { get; set; } = 1;

        // Foreign key
        protected SqlGuid LocationID { get; set; }

        public InventoryItem()
        {
            ID = new SqlGuid();
            Name = "";
            Notes = "";
        }

        public InventoryItem(SqlGuid? id, string name, string notes, string? fileImg1, string? fileImg2, string? fileImg3,
            int quantity)
        {
            ID = id;
            Name = name;
            Notes = notes;
            FileImage1 = fileImg1;
            FileImage2 = fileImg2;
            FileImage3 = fileImg3;
            Quantity = quantity;
        }

        public InventoryItem(string name, string notes, string? fileImg1, string? fileImg2, string? fileImg3,
            int quantity)
        {
            Name = name;
            Notes = notes;
            FileImage1 = fileImg1;
            FileImage2 = fileImg2;
            FileImage3 = fileImg3;
            Quantity = quantity;
        }

        public void Populate(int idToUse)
        {
            string IDStr = idToUse + "";
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["id"] = IDStr;
            PopulateHelper(d);
        }

        public void SetLocationID(SqlGuid locID)
        {
            LocationID = locID;
        }
    }
   
}   
