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
    public class OilCollection : PersistableCollection
    {
        protected List<Oil> OilList;

        protected delegate void PopulateQueryMethodType(Dictionary<string, Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public OilCollection()
        {
            OilList = new List<Oil>();
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
                string Nature = reader.GetString(reader.GetOrdinal("Nature"));
                SqlGuid BrandID = reader.GetSqlGuid(reader.GetOrdinal("BrandID"));
                Oil oil = new Oil(ID, Name, Notes, FileImage1, FileImage2, FileImage3, Quantity, Nature, BrandID);
                oil.SetLocationID(LocationID);
                OilList.Add(oil);

            }
        }

        public void PopulateAll()
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorAll);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            PopulateHelper(d);
        }

        public void PopulateViaName(string name)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaName);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["name"] = name;
            PopulateHelper(d);
        }

        public void PopulateViaBrandName(string brandName)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaBrandName);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["brandName"] = brandName;
            PopulateHelper(d);
        }

        public void PopulateViaBrandID(SqlGuid brandID)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaBrandID);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["brandID"] = brandID;
            PopulateHelper(d);
        }

        public void PopulateViaNature(string nature)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaNature);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["oilNature"] = nature;
            PopulateHelper(d);
        }

        public void PopulateViaBrandNameAndNature(string brandName, string nature)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaBrandNameAndNature);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["brandName"] = brandName;
            d["oilNature"] = nature;
            PopulateHelper(d);
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM Oil";

            command.CommandText = query;
        }

        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Oil WHERE (Name LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["name"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaBrandName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Oil INNER JOIN Brand ON
                            (Oil.BrandID = Brand.ID)
                            AND (Brand.Name LIKE CONCAT('%', @BRAND, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@BRAND", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["brandName"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaBrandID(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Oil WHERE BrandID = @BRANDID;";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@BRANDID", DT.SqlDbType.UniqueIdentifier, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["brandID"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaNature(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command) 
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Oil WHERE (Nature LIKE CONCAT('%', @NATURE, '%'));";
            command.CommandText = query;

            parameter = new QC.SqlParameter("@NATURE", DT.SqlDbType.NVarChar, 1000);
            parameter.Value = dictNotesPart["oilNature"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaBrandNameAndNature(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Oil INNER JOIN Brand ON
                            (Oil.BrandID = Brand.ID)
                            AND (Brand.Name LIKE CONCAT('%', @BRAND, '%'))
                            AND (Oil.Nature LIKE CONCAT('%', @NATURE, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@BRAND", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["brandName"];
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@NATURE", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["oilNature"];
            command.Parameters.Add(parameter);
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Oil Collection " +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving Oil Collection " +
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

            OilCollection other = (OilCollection)obj;

            if (OilList.Count != other.OilList.Count) { return false; }

            for (int cnt = 0; cnt < OilList.Count; cnt++)
            {
                Oil nextOil = OilList[cnt];
                Oil nextOtherOil = other.OilList[cnt];

                if (!nextOil.Equals(nextOtherOil)) { return false; }
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
            for (int cnt = 0; cnt < OilList.Count; cnt++)
            {
                retVal += OilList[cnt].ToString();
            }

            return retVal;
        }


    }





}
