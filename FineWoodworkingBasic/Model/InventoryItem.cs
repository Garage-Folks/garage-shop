using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{ 
    public abstract class InventoryItem : DeletablePersistable
    {
        public SqlGuid ID { get; protected set; } = new SqlGuid();
        public string Name { get; protected set; } = "";
        public string Notes { get; protected set; } = "";
        public string FileImage1 { get; protected set; } = "";
        public string FileImage2 { get; protected set; } = "";
        public string FileImage3 { get; protected set; } = "";
        public int Quantity { get; protected set; } = 1;

        // Foreign key
        public SqlGuid LocationID { get; protected set; } = new SqlGuid();

        public InventoryItem()
        {
        }

        public InventoryItem(SqlGuid id, string name, string notes, string fileImg1, string fileImg2, string fileImg3,
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

        public InventoryItem(string name, string notes, string fileImg1, string fileImg2, string fileImg3,
            int quantity)
        {
            Name = name;
            Notes = notes;
            FileImage1 = fileImg1;
            FileImage2 = fileImg2;
            FileImage3 = fileImg3;
            Quantity = quantity;
        }

        public void Populate(SqlGuid idToUse)
        {
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["id"] = idToUse;
            PopulateHelper(d);
        }

        public void SetLocationID(SqlGuid locID)
        {
            LocationID = locID;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;

            InventoryItem other = (InventoryItem)obj;

            if (!this.ID.Equals(other.ID)) return false;

            if (!this.Name.Equals(other.Name)) return false;

            if (!this.Notes.Equals(other.Notes)) return false;

            if (!this.FileImage1.Equals(other.FileImage1)) return false;

            if (!this.FileImage2.Equals(other.FileImage2)) return false;

            if (!this.FileImage3.Equals(other.FileImage3)) return false;

            if (this.Quantity != other.Quantity) return false;

            if (!this.LocationID.Equals(other.LocationID)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
   
}   
