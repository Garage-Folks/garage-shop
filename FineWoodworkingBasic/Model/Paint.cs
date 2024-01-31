﻿using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{
    public class Paint : InventoryItem
    {

        protected string MaterialType { get; set; }

        // Foreign Key
        protected SqlGuid BrandID { get; set; }

        public Paint(SqlGuid id, string name, string notes, string fileImg1, string fileImg2,
            string fileImg3, int quantity, string materialtype, SqlGuid BrandId) :
            base(id, name, notes, fileImg1, fileImg2, fileImg3, quantity)
        {
            MaterialType = materialtype;
            BrandID = BrandId;
        }

        public Paint(string name, string notes, string fileImg1, string fileImg2, string fileImg3,
            int quantity, string materialtype, SqlGuid BrandId) :
            base(name, notes, fileImg1, fileImg2, fileImg3, quantity)
        {
            MaterialType = materialtype;
            BrandID = BrandId;
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> dictIdToUse, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Paint WHERE (ID = @NP);";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.UniqueIdentifier);
            parameter.Value = dictIdToUse["id"];
            command.Parameters.Add(parameter);

        }

        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                ID = reader.GetSqlGuid(reader.GetOrdinal("ID"));
                LocationID = reader.GetSqlGuid(reader.GetOrdinal("LocationID"));
                BrandID = reader.GetSqlGuid(reader.GetOrdinal("BrandID"));
                Name = reader.GetString(reader.GetOrdinal("Name"));
                MaterialType = reader.GetString(reader.GetOrdinal("MaterialType"));
                FileImage1 = reader.GetString(reader.GetOrdinal("LinkImg1"));
                FileImage2 = reader.GetString(reader.GetOrdinal("LinkImg2"));
                FileImage3 = reader.GetString(reader.GetOrdinal("LinkImg3"));
                Quantity = reader.GetInt32(reader.GetOrdinal("Qty"));
                Notes = reader.GetString(reader.GetOrdinal("Notes"));
            }
        }

        public override bool IsPopulated()
        {
            if (this.ID == null) return false;
            if (this.ID.Equals(0)) return false;
            return true;
        }

        protected override void SetupCommandForInsert(QC.SqlCommand command)
        {
            // Taking a 'PreparedStatement' approach here, avoids SQL Injection  
            // THIS IS IMPORTANT 

            QC.SqlParameter parameter;

            string insertQuery = "INSERT INTO Paint (Name, Notes, LinkImg1, LinkImg2, LinkImg3, Qty, LocationID, MatertialType, BrandID) " +
                " OUTPUT INSERTED.ID " +
                " VALUES (@Name, @Notes, @LinkImg1, @LinkImg2, @LinkImg3, @Qty, @LocationID, @MaterialType, @BrandID);";

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

            parameter = new QC.SqlParameter("@MaterialType", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = MaterialType;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@BrandID", DT.SqlDbType.UniqueIdentifier, 1000); // Fix Type and Length  
            parameter.Value = BrandID;
            command.Parameters.Add(parameter);
        }

        protected override void SetupCommandForDelete(QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string deleteQuery = "DELETE FROM Paint " +
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

            string updateQuery = "UPDATE Paint" +
               " SET Name = @Name, Notes = @Notes, LinkImg1 = @LinkImg1, " +
               " LinkImg2 = @LinkImg2, LinkImg3 = @LinkImg3, Qty = @Qty, LocationID = @LocationID," +
               " MaterialType = @MaterialType, BrandID = @BrandID " +
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

            parameter = new QC.SqlParameter("@MaterialType", DT.SqlDbType.NVarChar, 1000); // Fix Type and Length  
            parameter.Value = MaterialType;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@BrandID", DT.SqlDbType.UniqueIdentifier, 1000); // Fix Type and Length  
            parameter.Value = BrandID;
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@Id", DT.SqlDbType.UniqueIdentifier);  // Fix Type and Length 
            parameter.Value = ID;
            command.Parameters.Add(parameter);
        }

        protected override bool IsNewObject()
        {
            if (ID == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Paint with ID: " + this.ID +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Paint with name: " + this.Name
                    + " saved successfully into database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving Paint with ID: " + this.ID +
                " from database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForSave(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in saving Paint with Name: " + this.Name +
                " into database!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForDelete()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Paint with name: " + this.Name
                    + " deleted successfully from database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForDelete(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in deleting Paint with Name: " + this.Name +
                " from database!");
            return mesg;
        }

        public override string ToString()
        {
            return "ID: " + ID + "; Name: " + Name + "; Notes: " + Notes + " ";
        }

    }
}
