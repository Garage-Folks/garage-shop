using System.Text;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{
    public class LocationConstraintCollection : PersistableCollection
    {
        protected List<LocationConstraint> LocationConstraintList;

        protected delegate void PopulateQueryMethodType(Dictionary<string,Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public LocationConstraintCollection()
        {
            LocationConstraintList = new List<LocationConstraint>();
            QueryMethod = QueryConstructorAll;
        }

        // Fully helper methods
        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                SqlGuid id = reader.GetSqlGuid(reader.GetOrdinal("ID"));
                string desc = reader.GetString(reader.GetOrdinal("Description"));
                LocationConstraintList.Add(new LocationConstraint(id, desc));
            }
        }

        public void PopulateAll()
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorAll);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            PopulateHelper(d);
        }

        public void PopulateViaDescription(string descPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaName);
            Dictionary<string,Object> d = new Dictionary<string,Object>();
            d["desc"] = descPart;
            PopulateHelper(d);
        }

        public void PopulateViaLocationAreaLocus(string areaPart, string locusPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaLocationAreaLocus);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["area"] = areaPart;
            d["locus"] = locusPart;
            PopulateHelper(d);
        }

        public void PopulateViaLocationID(SqlGuid locIDPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaLocationAreaLocus);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["locID"] = locIDPart;
            PopulateHelper(d);
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM LocationConstraint";

            command.CommandText = query;
        }

        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM LocationConstraint WHERE (Description LIKE CONCAT('%', @DESC, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@DESC", DT.SqlDbType.NVarChar, 100);  // Fix Type and Length 
            parameter.Value = dictNamePart["desc"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaLocationAreaLocus(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM LocationConstraint 
                             INNER JOIN ConstraintOfLocation 
                             ON (LocationConstraint.ID = ConstraintOfLocation.ConstraintID) 
                             INNER JOIN Location 
                             ON (ConstraintOfLocation.LocationID = Location.ID) 
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

        protected virtual void QueryConstructorViaLocationID(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM LocationConstraint 
                             INNER JOIN ConstraintOfLocation 
                             ON (LocationConstraint.ID = ConstraintOfLocation.ConstraintID) 
                             AND (ConstraintOfLocation.LocationID = @LOCID);";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@LOCID", DT.SqlDbType.UniqueIdentifier);  // Fix Type and Length 
            parameter.Value = dictNamePart["locID"];
            command.Parameters.Add(parameter);
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "LocationConstraint Collection "  +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving LocationConstraint Collection " +
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

            LocationConstraintCollection other = (LocationConstraintCollection)obj;

            if (LocationConstraintList.Count != other.LocationConstraintList.Count) { return false; }

            for (int cnt = 0; cnt < LocationConstraintList.Count; cnt++)
            {
                LocationConstraint nextLocationConstraint = LocationConstraintList[cnt];
                LocationConstraint nextOtherLocationConstraint = other.LocationConstraintList[cnt];

                if (!nextLocationConstraint.Equals(nextLocationConstraint)) { return false; }
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
            for (int cnt = 0; cnt < LocationConstraintList.Count; cnt++)
            {
                retVal += LocationConstraintList[cnt].ToString();
            }

            return retVal;
        }


    }





}
