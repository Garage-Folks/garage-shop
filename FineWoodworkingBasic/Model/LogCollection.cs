using System.Text;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace FineWoodworkingBasic.Model
{
    public class LogCollection : PersistableCollection
    {
        protected List<Log> LogList;

        protected delegate void PopulateQueryMethodType(Dictionary<string, Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public LogCollection()
        {
            LogList = new List<Log>();
            QueryMethod = QueryConstructorAll;
        }

        // Fully helper methods
        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                int ID = reader.GetInt32(reader.GetOrdinal("ID"));
                string Name = reader.GetString(reader.GetOrdinal("Name"));
                string Notes = reader.GetString(reader.GetOrdinal("Notes"));
                string FileImage1 = reader.GetString(reader.GetOrdinal("LinkImg1"));
                string FileImage2 = reader.GetString(reader.GetOrdinal("LinkImg2"));
                string FileImage3 = reader.GetString(reader.GetOrdinal("LinkImg3"));
                int Quantity = reader.GetInt32(reader.GetOrdinal("Qty"));
                int LocationID = reader.GetInt32(reader.GetOrdinal("LocationID"));
                double Length = reader.GetDouble(reader.GetOrdinal("Length"));
                double Diameter = reader.GetDouble(reader.GetOrdinal("Diameter"));
                int WoodSpeciesID = reader.GetInt32(reader.GetOrdinal("SpeciesWoodID"));
                Log log = new Log(ID, Name, Notes, FileImage1, FileImage2, FileImage3, Quantity, Length, Diameter, WoodSpeciesID);
                log.SetLocationID(LocationID);
                LogList.Add(log);

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

        public void PopulateViaWoodSpeciesID(string woodSpeciesIDPart)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaWoodSpeciesName);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["woodSpeciesID"] = woodSpeciesIDPart;
            PopulateHelper(d);
        }

        public void PopulateViaDimension(string dimension, double lowerLimit, double upperLimit)
        {
            if (!(dimension.Equals("length") || dimension.Equals("diameter")))
                throw new ArgumentException();
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaDimension);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["dimension"] = dimension;
            d["lowerLimitPart"] = lowerLimit;
            d["upperLimitPart"] = upperLimit;
            PopulateHelper(d);
        }

        public void PopulateViaMultiDimension(double lengthLower = -1, double lengthUpper = -1, 
            double diameterLower = -1, double diameterUpper = -1)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaMultiDimension);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            if (lengthLower <= 0 || lengthUpper <= 0)
            {
                d["lengthLowerPart"] = (lengthLower == -1) ? 0 : lengthLower;
                d["lengthUpperPart"] = (lengthUpper == -1) ? Int32.MaxValue : lengthUpper;
            }
            if (diameterLower <= 0 || diameterUpper <= 0)
            {
                d["diameterLowerPart"] = (diameterLower == -1) ? 0 : diameterLower;
                d["diameterUpperPart"] = (diameterUpper == -1) ? Int32.MaxValue : diameterUpper;
            }
            PopulateHelper(d);
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM Log";

            command.CommandText = query;
        }

        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Log WHERE (Name LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["name"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaWoodSpeciesName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Log INNER JOIN WoodSpecies ON
                            (Log.WoodSpeciesID = WoodSpecies.ID)
                            AND (WoodSpecies.Name LIKE CONCAT('%', @WSNP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@WSNP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesName"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaWoodSpeciesID(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Log WHERE SpeciesWoodID = @WSIDP;";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@WSIDP", DT.SqlDbType.Int, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesID"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaDimension(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Log WHERE @DIM BETWEEN @LOW AND @UP;";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@DIM", DT.SqlDbType.NVarChar, 1000);
            if (dictNotesPart["dimension"].Equals("length"))
                parameter.Value = "Length";
            else
                parameter.Value = "Diameter";
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@LOW", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["lowerLimitPart"];
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@UP", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["upperLimitPart"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaMultiDimension(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;
            bool firstSeg = true;

            string query = @"SELECT * FROM Log WHERE";
            if (dictNotesPart.ContainsKey("lengthLowerPart") && dictNotesPart.ContainsKey("lengthUpperPart"))
            {
                query += " Length BETWEEN @LENLOW AND @LENUP";
                firstSeg = false;
            }
            if (dictNotesPart.ContainsKey("diameterLowerPart") && dictNotesPart.ContainsKey("diameterUpperPart"))
            {
                if (!firstSeg) query += " AND";
                query += " Diameter BETWEEN @DIALOW AND @DIAUP";
            }
            query += ";";

            command.CommandText = query;

            if (dictNotesPart.ContainsKey("lengthLowerPart") && dictNotesPart.ContainsKey("lengthUpperPart"))
            {
                parameter = new QC.SqlParameter("@LENLOW", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
                parameter.Value = dictNotesPart["lengthLowerPart"];
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@LENUP", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
                parameter.Value = dictNotesPart["lengthUpperPart"];
                command.Parameters.Add(parameter);
            }
            if (dictNotesPart.ContainsKey("diameterLowerPart") && dictNotesPart.ContainsKey("diameterUpperPart"))
            {
                parameter = new QC.SqlParameter("@DIALOW", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
                parameter.Value = dictNotesPart["diameterLowerPart"];
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@DIAUP", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
                parameter.Value = dictNotesPart["diameterUpperPart"];
                command.Parameters.Add(parameter);
            }
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Log Collection " +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving Log Collection " +
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
            for (int cnt = 0; cnt < LogList.Count; cnt++)
            {
                retVal += LogList[cnt].ToString();
            }

            return retVal;
        }


    }





}
