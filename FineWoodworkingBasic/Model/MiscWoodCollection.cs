using System.Text;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{
    public class MiscWoodCollection : PersistableCollection
    {
        protected List<MiscWood> MiscWoodList;

        protected delegate void PopulateQueryMethodType(Dictionary<string, Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public MiscWoodCollection()
        {
            MiscWoodList = new List<MiscWood>();
            QueryMethod = QueryConstructorAll;
        }

        // Fully helper methods
        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                SqlGuid ID = reader.GetSqlGuid(reader.GetOrdinal("ID"));
                string Name = reader.GetString(reader.GetOrdinal("Name"));
                string Notes = reader.GetString(reader.GetOrdinal("Notes"));
                string FileImage1 = reader.GetString(reader.GetOrdinal("LinkImg1"));
                string FileImage2 = reader.GetString(reader.GetOrdinal("LinkImg2"));
                string FileImage3 = reader.GetString(reader.GetOrdinal("LinkImg3"));
                int Quantity = reader.GetInt32(reader.GetOrdinal("Qty"));
                SqlGuid LocationID = reader.GetSqlGuid(reader.GetOrdinal("LocationID"));
                string speciesDesc = reader.GetString(reader.GetOrdinal("SpeciesDesc"));
                SqlGuid WoodSpeciesID = reader.GetSqlGuid(reader.GetOrdinal("SpeciesWoodID"));
                MiscWood miscWood = new MiscWood(ID, Name, Notes, FileImage1, FileImage2, FileImage3, Quantity, speciesDesc, WoodSpeciesID);
                miscWood.SetLocationID(LocationID);
                MiscWoodList.Add(miscWood);

            }
        }

        public void PopulateAll()
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorAll);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            PopulateHelper(d);
        }

        public void PopulateViaName(string namePart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaName);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["name"] = namePart;
            PopulateHelper(d);
        }

        public void PopulateViaWoodSpeciesName(string woodSpeciesNamePart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaWoodSpeciesName);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["woodSpeciesName"] = woodSpeciesNamePart;
            PopulateHelper(d);
        }

        public void PopulateViaWoodSpeciesID(SqlGuid woodSpeciesIDPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaWoodSpeciesID);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["woodSpeciesID"] = woodSpeciesIDPart;
            PopulateHelper(d);
        }

        public void PopulateViaWoodSpeciesDescription(string woodSpeciesDescPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaWoodSpeciesDescription);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["woodSpeciesDesc"] = woodSpeciesDescPart;
            PopulateHelper(d);
        }

        public void PopulateViaWoodSpeciesNameAndDescription(string woodSpeciesNamePart, 
            string woodSpeciesDescPart) 
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaWoodSpeciesNameAndDescription);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["woodSpeciesName"] = woodSpeciesNamePart;
            d["woodSpeciesDesc"] = woodSpeciesDescPart;
            PopulateHelper(d);
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM MiscWood";

            command.CommandText = query;
        }

        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscWood WHERE (Name LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["name"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaWoodSpeciesName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscWood INNER JOIN WoodSpecies ON
                            (MiscWood.WoodSpeciesID = WoodSpecies.ID)
                            AND (WoodSpecies.Name LIKE CONCAT('%', @WSNP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@WSNP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesName"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaWoodSpeciesID(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscWood WHERE SpeciesWoodID = @WSIDP;";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@WSIDP", DT.SqlDbType.UniqueIdentifier, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesID"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaWoodSpeciesDescription(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscWood WHERE (SpeciesDesc LIKE CONCAT('%', @WSDP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@WSDP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesDesc"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaWoodSpeciesNameAndDescription(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscWood INNER JOIN WoodSpecies ON
                            (MiscWood.WoodSpeciesID = WoodSpecies.ID)
                            AND (WoodSpecies.Name LIKE CONCAT('%', @WSNP, '%'))
                            AND (MiscWood.SpeciesDesc LIKE CONCAT('%', @WSDP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@WSNP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesName"];
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@WSDP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesDesc"];
            command.Parameters.Add(parameter);
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "MiscWood Collection " +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving MiscWood Collection " +
                " from database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForSave(Exception Ex)
        {
            throw new NotSupportedException();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;

            MiscWoodCollection other = (MiscWoodCollection)obj;

            if (MiscWoodList.Count != other.MiscWoodList.Count) { return false; }

            for (int cnt = 0; cnt < MiscWoodList.Count; cnt++)
            {
                MiscWood nextMiscWood = MiscWoodList[cnt];
                MiscWood nextOtherMiscWood = other.MiscWoodList[cnt];

                if (!nextMiscWood.Equals(nextOtherMiscWood)) { return false; }
            }

            return true;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            string retVal = "";
            for (int cnt = 0; cnt < MiscWoodList.Count; cnt++)
            {
                retVal += MiscWoodList[cnt].ToString();
            }

            return retVal;
        }


    }





}
