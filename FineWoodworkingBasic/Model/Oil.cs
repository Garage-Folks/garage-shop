using System.Data.SqlTypes;
using QC = Microsoft.Data.SqlClient;
using DT = System.Data;
using Microsoft.Data.SqlClient;
using System;

namespace FineWoodworkingBasic.Model
{
    public class Oil : InventoryItem
    {
        protected string Nature { get; set; }

        // Foreign Key
        protected SqlGuid BrandID { get; set; }



        public Oil(SqlGuid id, string name, string notes, string fileImg1, string fileImg2,
            string fileImg3, int quantity, string nature, SqlGuid brandID) : 
            base(id, name, notes, fileImg1, fileImg2, fileImg3, quantity)
        {
            SetNature(nature);
            BrandID = brandID;
        }
        
        public Oil(string name, string notes, string fileImg1, string fileImg2,
            string fileImg3, int quantity, string nature, SqlGuid brandID) :
            base(name, notes, fileImg1, fileImg2, fileImg3, quantity)
        {
            SetNature(nature);
            BrandID = brandID;
        }

        private void SetNature(string nature)
        {
            string uppercaseNature = nature.ToUpper();

            if (uppercaseNature == "DRYING" || uppercaseNature == "NON-DRYING")
            {
                Nature = uppercaseNature;
            } else
            {
                throw new ArgumentException("Invalid value for Nature. Must be 'Drying' or 'Non-Drying'.");
            }
        }

        public override bool IsPopulated()
        {
            if (this.ID == null) return false;
            if (this.ID.Equals(0)) return false;
            return true;
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, object> dictIdToUse, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;
            String query = @"SELECT * FROM Oil WHERE (ID = @NP);";

            command.CommandText = query;
            parameter = new SqlParameter("@NP", DT.SqlDbType.UniqueIdentifier);
            parameter.Value = dictIdToUse["id"];
            command.Parameters.Add(parameter);
        }

        protected override ResultMessage GetErrorMessageForDelete(Exception excep)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in deleting Oil with Name: " + this.Name
                    + " from the database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception excep)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving Oil with ID: " + this.ID
                    + " from the database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForSave(Exception excep)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in saving Oil with Name: " + this.Name
                    + " into the database!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForDelete()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Oil with Name: " + this.Name
                    + " deleted successfully from the database!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Oil with ID: " + this.ID
                    + " retrieved successfully from the database!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Oil with Name: " + this.Name
                    + " saved successfully into the database!");
            return mesg;
        }

        protected override bool IsNewObject()
        {
            if (ID == null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                ID = reader.GetSqlGuid(reader.GetOrdinal("ID"));
                LocationID = reader.GetSqlGuid(reader.GetOrdinal("LocationID"));
                BrandID = reader.GetSqlGuid(reader.GetOrdinal("BrandID"));
                Name = reader.GetString(reader.GetOrdinal("Name"));
                Nature = reader.GetString(reader.GetOrdinal("Nature"));
                FileImage1 = reader.GetString(reader.GetOrdinal("LinkImg1"));
                FileImage2 = reader.GetString(reader.GetOrdinal("LinkImg2"));
                FileImage3 = reader.GetString(reader.GetOrdinal("LinkImg3"));
                Quantity = reader.GetInt32(reader.GetOrdinal("Qty"));
                Notes = reader.GetString(reader.GetOrdinal("Notes"));
            }
        }

        protected override void SetupCommandForDelete(QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string deleteQuery = "DELETE FROM Oil " +
                " (WHERE ID = @ID)";

            command.CommandText = deleteQuery;

            parameter = new QC.SqlParameter("@ID", DT.SqlDbType.UniqueIdentifier);  // Fix Type and Length 
            parameter.Value = this.ID;
            command.Parameters.Add(parameter);
        }

        protected override void SetupCommandForInsert(QC.SqlCommand command)
        {
            // Taking a 'PreparedStatement' approach here, avoids SQL Injection  
            // THIS IS IMPORTANT 

            QC.SqlParameter parameter;

            string insertQuery = "INSERT INTO Oil (Name, Notes, LinkImg1, LinkImg2, LinkImg3, Qty, LocationID, Nature, BrandedID) " +
                " OUTPUT INSERTED.ID " +
                " VALUES (@Name, @Notes, @LinkImg1, @LinkImg2, @LinkImg3, @Qty, @LocationID, @Nature, @BrandID);";

            command.CommandText = insertQuery;

            parameter = new QC.SqlParameter("@Name", DT.SqlDbType.NVarChar, 100);  // Fix Type and Length 
            parameter.Value = Name;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Notes", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = Notes;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg1", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = FileImage1;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg2", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = FileImage2;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg3", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = FileImage3;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Qty", DT.SqlDbType.Int, 1000); // Fix Type and Length  
            parameter.Value = Quantity;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LocationID", DT.SqlDbType.UniqueIdentifier, 1000); // Fix Type and Length  
            parameter.Value = LocationID;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@BrandID", DT.SqlDbType.UniqueIdentifier, 1000); // Fix Type and Length  
            parameter.Value = BrandID;
            command.Parameters.Add(parameter);
        }

        protected override void SetAutogeneratedIDFromInsert(SqlGuid genID)
        {
            this.ID = genID;
        }

        protected override void SetupCommandForUpdate(QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string updateQuery = "UPDATE Oil" +
               " SET Name = @Name, Notes = @Notes, LinkImg1 = @LinkImg1, " +
               " LinkImg2 = @LinkImg2, LinkImg3 = @LinkImg3, Qty = @Qty, LocationID = @LocationID," +
               " Nature = @Nature, BrandID = @BrandID " +
               " WHERE (ID = @Id);";

            command.CommandText = updateQuery;

            parameter = new QC.SqlParameter("@Name", DT.SqlDbType.NVarChar, 100);  // Fix Type and Length 
            parameter.Value = Name;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Notes", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = Notes;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg1", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = FileImage1;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg2", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = FileImage2;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LinkImg3", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = FileImage3;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Qty", DT.SqlDbType.Int, 1000); // Fix Type and Length  
            parameter.Value = Quantity;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LocationID", DT.SqlDbType.UniqueIdentifier, 1000); // Fix Type and Length  
            parameter.Value = LocationID;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Nature", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = Nature;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@BrandID", DT.SqlDbType.UniqueIdentifier, 1000); // Fix Type and Length  
            parameter.Value = BrandID;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Id", DT.SqlDbType.UniqueIdentifier);  // Fix Type and Length 
            parameter.Value = ID;
            command.Parameters.Add(parameter);
        }

    }
}
