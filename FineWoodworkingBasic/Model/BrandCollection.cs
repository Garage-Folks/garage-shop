using System.Text;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{
    public class BrandCollection : PersistableCollection
    {
        public List<Brand> BrandList;

        protected delegate void PopulateQueryMethodType(Dictionary<string,Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public BrandCollection()
        {
            BrandList = new List<Brand>();
            QueryMethod = QueryConstructorAll;
        }

        // Fully helper methods
        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                SqlGuid id = reader.GetSqlGuid(reader.GetOrdinal("ID"));
                string name = reader.GetString(reader.GetOrdinal("Name"));
                string notes = reader.GetString(reader.GetOrdinal("Notes"));
                BrandList.Add(new Brand(id, name, notes));
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
            Dictionary<string,Object> d = new Dictionary<string,Object>();
            d["name"] = namePart;
            PopulateHelper(d);
        }

        public void PopulateViaNotes(string notesPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaNotes);
            Dictionary<string,Object> d = new Dictionary<string,Object>();
            d["notes"] = notesPart;
            PopulateHelper(d);
        }
        public void PopulateViaNameAndNotes(string namePart, string notesPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaNameAndNotes);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["name"] = namePart;
            d["notes"] = notesPart;
            PopulateHelper(d);
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM Brand";

            command.CommandText = query;
        }

        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Brand WHERE (Name LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 50);  // Fix Type and Length 
            parameter.Value = dictNamePart["name"];
            command.Parameters.Add(parameter);
        }


        protected virtual void QueryConstructorViaNotes(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Brand WHERE (Notes LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 2000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["notes"];
            command.Parameters.Add(parameter);
        }
        protected virtual void QueryConstructorViaNameAndNotes(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Brand WHERE Notes LIKE CONCAT('%', @NP1, '%') AND Name LIKE CONCAT('%', @NP2, '%');";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP1", DT.SqlDbType.NVarChar, 2000);
            parameter.Value = dictNotesPart["notes"];
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@NP2", DT.SqlDbType.NVarChar, 50); 
            parameter.Value = dictNotesPart["name"];
            command.Parameters.Add(parameter);
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Brand Collection "  +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving Brand Collection " +
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

            BrandCollection other = (BrandCollection)obj;

            if (BrandList.Count != other.BrandList.Count) { return false; }

            for (int cnt = 0; cnt < BrandList.Count; cnt++)
            {
                Brand nextBrand = BrandList[cnt];
                Brand nextOtherBrand = other.BrandList[cnt];

                if (!nextBrand.Equals(nextOtherBrand)) { return false; }
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
            for (int cnt = 0; cnt < BrandList.Count; cnt++)
            {
                retVal += BrandList[cnt].ToString();
            }

            return retVal;
        }
    }
}
