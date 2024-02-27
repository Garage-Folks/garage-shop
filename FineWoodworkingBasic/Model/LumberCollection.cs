using System.Text;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace FineWoodworkingBasic.Model
{
    public class LumberCollection : PersistableCollection
    {
        protected List<Lumber> LumberList;

        protected delegate void PopulateQueryMethodType(Dictionary<string, Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public LumberCollection()
        {
            LumberList = new List<Lumber>();
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
                double Length = reader.GetDouble(reader.GetOrdinal("Length"));
                double Width = reader.GetDouble(reader.GetOrdinal("Width"));
                double Thickness = reader.GetDouble(reader.GetOrdinal("Thickness"));
                SqlGuid WoodSpeciesID = reader.GetSqlGuid(reader.GetOrdinal("SpeciesWoodID"));
                Lumber lumber = new Lumber(ID, Name, Notes, FileImage1, FileImage2, FileImage3, Quantity, Length, Width, Thickness, WoodSpeciesID);
                lumber.SetLocationID(LocationID);
                LumberList.Add(lumber);

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
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaWoodSpeciesName);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["woodSpeciesID"] = woodSpeciesIDPart;
            PopulateHelper(d);
        }

        public void PopulateViaDimension(string dimension, double lowerLimit, double upperLimit)
        {
            if (!(dimension.Equals("length") || dimension.Equals("width") || dimension.Equals("thickness")))
                throw new ArgumentException();
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaDimension);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["dimension"] = dimension;
            d["lowerLimitPart"] = lowerLimit;
            d["upperLimitPart"] = upperLimit;
            PopulateHelper(d);
        }

        public void PopulateViaMultiDimension(double lengthLower = -1, double lengthUpper = -1, double widthLower = -1,
            double widthUpper = -1, double thicknessLower = -1, double thicknessUpper = -1)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaMultiDimension);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            if (lengthLower <= 0 || lengthUpper <= 0)
            {
                d["lengthLowerPart"] = (lengthLower == -1) ? 0 : lengthLower;
                d["lengthUpperPart"] = (lengthUpper == -1) ? Int32.MaxValue : lengthUpper;
            }
            if (widthLower <= 0 || widthUpper <= 0)
            {
                d["widthLowerPart"] = (widthLower == -1) ? 0 : widthLower;
                d["widthUpperPart"] = (widthUpper == -1) ? Int32.MaxValue : widthUpper;
            }
            if (thicknessLower <= 0 || thicknessUpper <= 0)
            {
                d["thicknessLowerPart"] = (thicknessLower == -1) ? 0 : thicknessLower;
                d["thicknessUpperPart"] = (thicknessUpper == -1) ? Int32.MaxValue : thicknessUpper;
            }
            PopulateHelper(d);
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM Lumber";

            command.CommandText = query;
        }

        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Lumber WHERE (Name LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["name"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaWoodSpeciesName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Lumber INNER JOIN WoodSpecies ON
                            (Lumber.WoodSpeciesID = WoodSpecies.ID)
                            AND (WoodSpecies.Name LIKE CONCAT('%', @WSNP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@WSNP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesName"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaWoodSpeciesID(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Lumber WHERE SpeciesWoodID = @WSIDP;";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@WSIDP", DT.SqlDbType.UniqueIdentifier, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["woodSpeciesID"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaDimension(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Lumber WHERE @DIM BETWEEN @LOW AND @UP;";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@DIM", DT.SqlDbType.NVarChar, 1000);
            if (dictNotesPart["dimension"].Equals("length"))
                parameter.Value = "Length";
            else if (dictNotesPart["dimension"].Equals("width"))
                parameter.Value = "Width";
            else
                parameter.Value = "Thickness";
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

            string query = @"SELECT * FROM Lumber WHERE ";
            if (dictNotesPart.ContainsKey("lengthLowerPart") && dictNotesPart.ContainsKey("lengthUpperPart"))
            {
                query += " Length BETWEEN @LENLOW AND @LENUP";
                firstSeg = false;
            }
            if (dictNotesPart.ContainsKey("widthLowerPart") && dictNotesPart.ContainsKey("widthUpperPart"))
            {
                if (!firstSeg) query += " AND";
                else firstSeg = false;
                query += " Width BETWEEN @WIDLOW AND @WIDUP";
            }
            if (dictNotesPart.ContainsKey("thicknessLowerPart") && dictNotesPart.ContainsKey("thicknessUpperPart"))
            {
                if (!firstSeg) query += " AND";
                query += " Thickness BETWEEN @THILOW AND @THIUP";
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
            if (dictNotesPart.ContainsKey("widthLowerPart") && dictNotesPart.ContainsKey("widthUpperPart"))
            {
                parameter = new QC.SqlParameter("@WIDLOW", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
                parameter.Value = dictNotesPart["widthLowerPart"];
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@WIDUP", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
                parameter.Value = dictNotesPart["widthUpperPart"];
                command.Parameters.Add(parameter);
            }
            if (dictNotesPart.ContainsKey("thicknessLowerPart") && dictNotesPart.ContainsKey("thicknessUpperPart"))
            {
                parameter = new QC.SqlParameter("@THILOW", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
                parameter.Value = dictNotesPart["thicknessLowerPart"];
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@THIUP", DT.SqlDbType.Float, 1000);  // Fix Type and Length 
                parameter.Value = dictNotesPart["thicknessUpperPart"];
                command.Parameters.Add(parameter);
            }
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Lumber Collection " +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving Lumber Collection " +
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

            LumberCollection other = (LumberCollection)obj;

            if (LumberList.Count != other.LumberList.Count) { return false; }

            for (int cnt = 0; cnt < LumberList.Count; cnt++)
            {
                Lumber nextLumber = LumberList[cnt];
                Lumber nextOtherLumber = other.LumberList[cnt];

                if (!nextLumber.Equals(nextOtherLumber)) { return false; }
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
            for (int cnt = 0; cnt < LumberList.Count; cnt++)
            {
                retVal += LumberList[cnt].ToString();
            }

            return retVal;
        }


    }





}
