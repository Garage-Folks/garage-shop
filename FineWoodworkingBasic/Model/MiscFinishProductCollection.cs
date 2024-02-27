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
    public class MiscFinishProductCollection : PersistableCollection
    {
        public List<MiscFinishProduct> MFPList;

        protected delegate void PopulateQueryMethodType(Dictionary<string, Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public MiscFinishProductCollection()
        {
            MFPList = new List<MiscFinishProduct>();
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
                string MaterialType = reader.GetString(reader.GetOrdinal("MaterialType"));
                SqlGuid BrandID = reader.GetSqlGuid(reader.GetOrdinal("BrandID"));
                MiscFinishProduct mfp = new MiscFinishProduct(ID, Name, Notes, FileImage1, FileImage2, FileImage3, Quantity, MaterialType, BrandID);
                mfp.SetLocationID(LocationID);
                MFPList.Add(mfp);

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

        public void PopulateViaMFPMaterialType(string mfpMaterialType)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaMaterialType);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["mfpMaterialType"] = mfpMaterialType;
            PopulateHelper(d);
        }

        public void PopulateViaBrandNameAndMaterialType(string brandName,
            string mfpMaterialType)
        {
            QueryMethod = new PopulateQueryMethodType(QueryConstructorViaBrandNameAndMaterialType);
            Dictionary<string, Object> d = new Dictionary<string, Object>();
            d["brandName"] = brandName;
            d["mfpMaterialType"] = mfpMaterialType;
            PopulateHelper(d);
        }

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM MiscFinishProduct";

            command.CommandText = query;
        }

        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscFinishProduct WHERE (Name LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 50);  // Fix Type and Length 
            parameter.Value = dictNotesPart["name"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaBrandName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscFinishProduct INNER JOIN Brand ON
                            (MiscFinishProduct.BrandID = Brand.ID)
                            AND (Brand.Name LIKE CONCAT('%', @BRAND, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@BRAND", DT.SqlDbType.NVarChar, 50);  // Fix Type and Length 
            parameter.Value = dictNotesPart["brandName"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaBrandID(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscFinishProduct WHERE BrandID = @BRANDID;";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@BRANDID", DT.SqlDbType.UniqueIdentifier);  // Fix Type and Length 
            parameter.Value = dictNotesPart["brandID"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaMaterialType(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscFinishProduct WHERE (MaterialType LIKE CONCAT('%', @MFPMT, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@MFPMT", DT.SqlDbType.NVarChar, 50);  // Fix Type and Length 
            parameter.Value = dictNotesPart["mfpMaterialType"];
            command.Parameters.Add(parameter);
        }

        protected virtual void QueryConstructorViaBrandNameAndMaterialType(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM MiscFinishProduct INNER JOIN Brand ON
                            (Paint.BrandID = Brand.ID)
                            AND (Brand.Name LIKE CONCAT('%', @BRAND, '%'))
                            AND (MiscFinishProduct.MaterialType LIKE CONCAT('%', @MFPMT, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@BRAND", DT.SqlDbType.NVarChar, 50);  // Fix Type and Length 
            parameter.Value = dictNotesPart["brandName"];
            command.Parameters.Add(parameter);

            parameter = new QC.SqlParameter("@MFPMT", DT.SqlDbType.NVarChar, 50);  // Fix Type and Length 
            parameter.Value = dictNotesPart["mfpMaterialType"];
            command.Parameters.Add(parameter);
        }

        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "MiscFinishProduct Collection " +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving MiscFinishProduct Collection " +
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

            MiscFinishProductCollection other = (MiscFinishProductCollection)obj;

            if (MFPList.Count != other.MFPList.Count) { return false; }

            for (int cnt = 0; cnt < MFPList.Count; cnt++)
            {
                MiscFinishProduct nextMFP = MFPList[cnt];
                MiscFinishProduct nextOtherMFP = other.MFPList[cnt];

                if (!nextMFP.Equals(nextOtherMFP)) { return false; }
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
            for (int cnt = 0; cnt < MFPList.Count; cnt++)
            {
                retVal += MFPList[cnt].ToString();
            }

            return retVal;
        }


    }





}