using System.Globalization;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{
    public class LocationCollection : PersistableCollection
    {
        protected List<Location> LocationList;

        protected delegate void PopulateQueryMethodType(Dictionary<string, Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public LocationCollection()
        {
            LocationList = new List<Location>();
            QueryMethod = QueryConstructorAll;
        }

        // Fully helper methods
        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                SqlGuid ID = reader.GetSqlGuid(reader.GetOrdinal("ID"));
                string area = reader.GetString(reader.GetOrdinal("Area"));
                string locus = reader.GetString(reader.GetOrdinal("Locus"));
                LocationList.Add(new Location(ID, area, locus));
            }
        }

        public void PopulateAll()
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorAll);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            PopulateHelper(d);
        }

       

   /*     public void PopulateViaLocationAreaLocus(string areaPart, string locusPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaLocationAreaLocus);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["area"] = areaPart;
            d["locus"] = locusPart;
            PopulateHelper(d);
        }*/
    

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM Location";

            command.CommandText = query;
        }

       /*
        protected virtual void QueryConstructorViaLocationAreaLocus(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM LocationConstraint 
                             INNER JOIN Location 
                             INNER JOIN ConstraintOfLocation 
                             ON (LocationConstraint.ID = ConstraintOfLocation.ConstraintID) 
                             AND (ConstraintOfLocation.LocationID = Location.ID) 
                             AND (Location.Area LIKE CONCAT('%', @AREA, '%')) 
                             AND (Location.Locus LIKE CONCAT('%', @LOCUS, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@AREA", DT.SqlDbType.NVarChar, 10);  // Fix Type and Length 
            parameter.Value = dictNamePart["area"];
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LOCUS", DT.SqlDbType.NVarChar, 25);  // Fix Type and Length 
            parameter.Value = dictNamePart["locus"];
            command.Parameters.Add(parameter);
        } */
        
        

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Location Collection " +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving Location Collection " +
                " from database!");
            return mesg;
        }

        protected override ResultMessage GetErrorMessageForSave(Exception Ex)
        {
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            string retVal = "";
            for (int cnt = 0; cnt < LocationList.Count; cnt++)
            {
                retVal += LocationList[cnt].ToString();
            }

            return retVal;
        }
    }
}
