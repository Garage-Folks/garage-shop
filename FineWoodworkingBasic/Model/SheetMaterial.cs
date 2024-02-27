using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{
    public class SheetMaterial : InventoryItem
    {
       
        public double Length { get; protected set; }
        public double Width { get; protected set; }
        public double Thickness { get; protected set; } 
        
        public SheetMaterial(SqlGuid id, string name, string notes, string fileImg1, string fileImg2, string fileImg3,
            int quantity, double length, double width, double thickness) : 
            base(id, name, notes, fileImg1, fileImg2, fileImg3, quantity)
        {
            Length = length;
            Width = width;
            Thickness = thickness;
        }

        public SheetMaterial(string name, string notes, string fileImg1, string fileImg2, string fileImg3,
            int quantity, double length, double width, double thickness) :
            base(name, notes, fileImg1, fileImg2, fileImg3, quantity)
        {
            Length = length;
            Width = width;
            Thickness = thickness;
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> dictIdToUse, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM SheetMaterial WHERE (ID = @Id);";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@Id", DT.SqlDbType.UniqueIdentifier);
            parameter.Value = dictIdToUse["id"];
            command.Parameters.Add(parameter);

        }

        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                ID = reader.GetSqlGuid(reader.GetOrdinal("ID"));
                Name = reader.GetString(reader.GetOrdinal("Name"));
                Notes = reader.GetString(reader.GetOrdinal("Notes"));
                FileImage1 = reader.GetString(reader.GetOrdinal("LinkImg1"));
                FileImage2 = reader.GetString(reader.GetOrdinal("LinkImg2"));
                FileImage3 = reader.GetString(reader.GetOrdinal("LinkImg3"));
                Quantity = reader.GetInt32(reader.GetOrdinal("Qty"));
                LocationID = reader.GetSqlGuid(reader.GetOrdinal("LocationID"));
                Length = reader.GetDouble(reader.GetOrdinal("Length"));
                Width = reader.GetDouble(reader.GetOrdinal("Width"));
                Thickness = reader.GetDouble(reader.GetOrdinal("Thickness"));
            }
        }

        public override bool IsPopulated()
        {
            if (this.ID.IsNull) return false;
            return true;
        }

        protected override void SetupCommandForInsert(QC.SqlCommand command)
        {
            // Taking a 'PreparedStatement' approach here, avoids SQL Injection  
            // THIS IS IMPORTANT 

            QC.SqlParameter parameter;

            string insertQuery = "INSERT INTO SheetMaterial (Name, Notes, LinkImg1, LinkImg2, LinkImg3, Qty, LocationID, Length, Width, Thickness ) " +
                " OUTPUT INSERTED.ID " +
                " VALUES (@Name, @Notes, @LinkImg1, @LinkImg2, @LinkImg3, @Qty, @LocationID, @Length, @Width, @Thickness);";

            command.CommandText = insertQuery;

            parameter = new QC.SqlParameter("@Name", DT.SqlDbType.NVarChar, 50);  // Fix Type and Length 
            parameter.Value = Name;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Notes", DT.SqlDbType.NVarChar, 2000); // Fix Type and Length  
            parameter.Value = Notes;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg1", DT.SqlDbType.NVarChar); // Fix Type and Length  
            parameter.Value = FileImage1;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg2", DT.SqlDbType.NVarChar); // Fix Type and Length  
            parameter.Value = FileImage2;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg3", DT.SqlDbType.NVarChar); // Fix Type and Length  
            parameter.Value = FileImage3;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Qty", DT.SqlDbType.Int, 5); // Fix Type and Length  
            parameter.Value = Quantity;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LocationID", DT.SqlDbType.UniqueIdentifier); // Fix Type and Length  
            parameter.Value = LocationID;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Length", DT.SqlDbType.Float, 7); // Fix Type and Length  
            parameter.Value = Length;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Width", DT.SqlDbType.Float, 7); // Fix Type and Length  
            parameter.Value = Width;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Thickness", DT.SqlDbType.Float, 7); // Fix Type and Length  
            parameter.Value = Thickness;
            command.Parameters.Add(parameter);
        }

        protected override void SetupCommandForDelete(QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string deleteQuery = "DELETE FROM SheetMaterial " +
                " (WHERE ID = @ID)";

            command.CommandText = deleteQuery;

            parameter = new QC.SqlParameter("@ID", DT.SqlDbType.UniqueIdentifier);  // Fix Type and Length 
            parameter.Value = this.ID;
            command.Parameters.Add(parameter);
        }

        protected override void SetAutogeneratedIDFromInsert(SqlGuid genID)
        {
            this.ID = genID;
        }

        protected override void SetupCommandForUpdate(QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string updateQuery = "UPDATE SheetMaterial" +
               " SET Name = @Name, Notes = @Notes, LinkImg1 = @LinkImg1, " +
               " LinkImg2 = @LinkImg2, LinkImg3 = @LinkImg3, Qty = @Qty, LocationID = @LocationID," +
               " Length = @Length, Width = @Width, Thickness = @Thickness" +
               " WHERE (ID = @Id);";

            command.CommandText = updateQuery;

            parameter = new QC.SqlParameter("@Name", DT.SqlDbType.NVarChar, 50);  // Fix Type and Length 
            parameter.Value = Name;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Notes", DT.SqlDbType.NVarChar, 2000); // Fix Type and Length  
            parameter.Value = Notes;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg1", DT.SqlDbType.NVarChar); // Fix Type and Length  
            parameter.Value = FileImage1;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg2", DT.SqlDbType.NVarChar); // Fix Type and Length  
            parameter.Value = FileImage2;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg3", DT.SqlDbType.NVarChar); // Fix Type and Length  
            parameter.Value = FileImage3;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Qty", DT.SqlDbType.Int, 5); // Fix Type and Length  
            parameter.Value = Quantity;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LocationID", DT.SqlDbType.UniqueIdentifier); // Fix Type and Length  
            parameter.Value = LocationID;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Length", DT.SqlDbType.Float, 7); // Fix Type and Length  
            parameter.Value = Length;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Width", DT.SqlDbType.Float, 7); // Fix Type and Length  
            parameter.Value = Width;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Thickness", DT.SqlDbType.Float, 7); // Fix Type and Length  
            parameter.Value = Thickness;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Id", DT.SqlDbType.UniqueIdentifier);  // Fix Type and Length 
            parameter.Value = ID;
            command.Parameters.Add(parameter);
        }

        protected override bool IsNewObject()
        {
            return !this.IsPopulated();
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "SheetMaterial with Name: " + this.Name +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "SheetMaterial with Name: " + this.Name
                    + " saved successfully into database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving SheetMaterial with Name: " + this.Name +
                " from database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForSave(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in saving SheetMaterial with Name: " + this.Name +
                " into database!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForDelete()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "SheetMaterial with Name: " + this.Name
                    + " deleted successfully from database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForDelete(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in deleting SheetMaterial with Name: " + this.Name +
                " from database!");
            return mesg;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;

            SheetMaterial other = (SheetMaterial)obj;

            if (!base.Equals((InventoryItem)other)) return false;

            if (this.Length != other.Length) return false;

            if (this.Width != other.Width) return false;

            if (this.Thickness != other.Thickness) return false;

            return true;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "\nSheet Material\n----------\n" +
                   $"   Name: {Name}\n" +
                   $"   Quantity: {Quantity}\n" +
                   $"   Length: {Length}\n" +
                   $"   Width: {Width}\n" +
                   $"   Thickness: {Thickness}";
        }

    }
}
