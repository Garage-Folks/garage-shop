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


        public void PopulateViaArea(string areaPart)
         {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaArea);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["area"] = areaPart;
            PopulateHelper(d);
         }


        public void PopulateViaAreaLocus(string areaPart, string locusPart)
        { 
            QueryMethod = new PopulateQueryMethodType (QueryConstructorViaAreaLocus);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["area"] = areaPart;
            d["locus"] = locusPart;
            PopulateHelper(d);
        }

        public void PopulateViaLocationConstraint(string constraintID)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaLocationConstraint);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["CONSTRAINTID"] = constraintID;
            PopulateHelper(d);
        }


        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM Location";

            command.CommandText = query;
        }

        
        protected virtual void QueryConstructorViaArea(Dictionary<string, Object> dictAreaPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Location 
                            WHERE (Location.Area LIKE CONCAT('%', @AREA, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@AREA", DT.SqlDbType.NVarChar, 10);  // Fix Type and Length 
            parameter.Value = dictAreaPart["area"];
            command.Parameters.Add(parameter);
        }


        
        protected virtual void QueryConstructorViaAreaLocus(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Location 
                             WHERE ((Location.Area LIKE CONCAT('%', @AREA, '%')) 
                             AND (Location.Locus LIKE CONCAT('%', @LOCUS, '%')));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@AREA", DT.SqlDbType.NVarChar, 10);  // Fix Type and Length 
            parameter.Value = dictNamePart["area"];
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LOCUS", DT.SqlDbType.NVarChar, 25);  // Fix Type and Length 
            parameter.Value = dictNamePart["locus"];
            command.Parameters.Add(parameter);
        } 
        
        protected virtual void QueryConstructorViaLocationConstraint(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
             QC.SqlParameter parameter;

             string query = @"SELECT * FROM Location 
	                        INNER JOIN ConstraintOfLocation 
	                        ON (Location.ID = ConstraintOfLocation.LocationID)
                            INNER JOIN LocationConstraint
                            ON (LocationConstraint.ID = ConstraintOfLocation.ConstraintID)                              
                            WHERE (LocationConstraint.ID = @CONSTRAINTID);";

             command.CommandText = query;

             parameter = new QC.SqlParameter("@CONSTRAINTID", DT.SqlDbType.NVarChar, 10);  // Fix Type and Length 
             parameter.Value = dictNamePart["CONSTRAINTID"];
             command.Parameters.Add(parameter);
        } 



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
