﻿using System.Text;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace FineWoodworkingBasic.Model
{
    public class BrandCollection : PersistableCollection
    {
        protected List<Brand> BrandList;

        protected delegate void PopulateQueryMethodType(Dictionary<string,Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public BrandCollection()
        {
            BrandList = new List<Brand>();
            QueryMethod = QueryConstructorViaName;
        }

        // Fully helper methods
        protected override void ProcessPopulateQueryResult(QC.SqlDataReader reader)
        {
            while (reader.Read())
            {
                int locID = reader.GetInt32(0);
                string locName = reader.GetString(1);
                string locNotes = reader.GetString(2);
                BrandList.Add(new Brand(locID, locName, locNotes));
            }
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

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Brand WHERE (Name LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 100);  // Fix Type and Length 
            parameter.Value = dictNamePart["name"];
            command.Parameters.Add(parameter);
        }


        protected virtual void QueryConstructorViaNotes(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Brand WHERE (Notes LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 1000);  // Fix Type and Length 
            parameter.Value = dictNotesPart["notes"];
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
