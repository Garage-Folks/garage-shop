﻿using System.Text;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;
using FineWoodworkingBasic.Util;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;

namespace FineWoodworkingBasic.Model
{
    public class ToolCollection : PersistableCollection
    {
        protected List<Tool> ToolList;

        protected delegate void PopulateQueryMethodType(Dictionary<string, Object> val, QC.SqlCommand command);

        protected PopulateQueryMethodType QueryMethod;

        public ToolCollection()
        {
            ToolList = new List<Tool>();
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
                string ToolType = reader.GetString(reader.GetOrdinal("ToolType"));
                SqlGuid BrandID = reader.GetSqlGuid(reader.GetOrdinal("BrandID"));
                Tool tool = new Tool(ID, Name, Notes, FileImage1, FileImage2, FileImage3, Quantity, ToolType, BrandID);
                tool.SetLocationID(LocationID);
                ToolList.Add(tool);
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

        protected override void ConstructPopulateQueryCommand(Dictionary<string, Object> val, QC.SqlCommand command)
        {
            QueryMethod(val, command);
        }

        protected virtual void QueryConstructorAll(Dictionary<string, Object> dictNamePart, QC.SqlCommand command)
        {
            string query = @"SELECT * FROM Tool";

            command.CommandText = query;
        }


        protected virtual void QueryConstructorViaName(Dictionary<string, Object> dictNotesPart, QC.SqlCommand command)
        {
            QC.SqlParameter parameter;

            string query = @"SELECT * FROM Tool WHERE (Name LIKE CONCAT('%', @NP, '%'));";

            command.CommandText = query;

            parameter = new QC.SqlParameter("@NP", DT.SqlDbType.NVarChar, 50);
            parameter.Value = dictNotesPart["name"];
            command.Parameters.Add(parameter);
        }


        protected override ResultMessage GetResultMessageForPopulate()
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Success, "Tool Collection " +
                " retrieved successfully!");
            return mesg;
        }

        protected override ResultMessage GetResultMessageForSave()
        {
            throw new NotImplementedException();
        }

        protected override ResultMessage GetErrorMessageForPopulate(Exception Ex)
        {
            ResultMessage mesg = new ResultMessage(ResultMessage.ResultMessageType.Error, "Error in retrieving Tool Collection " +
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

            ToolCollection other = (ToolCollection)obj;

            if (ToolList.Count != other.ToolList.Count) { return false; }

            for (int cnt = 0; cnt < ToolList.Count; cnt++)
            {
                Tool nextTool = ToolList[cnt];
                Tool nextOtherTool = other.ToolList[cnt];

                if (!nextTool.Equals(nextOtherTool)) { return false; }
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
            for (int cnt = 0; cnt < ToolList.Count; cnt++)
            {
                retVal += ToolList[cnt].ToString();
            }

            return retVal;
        }


    }





}
