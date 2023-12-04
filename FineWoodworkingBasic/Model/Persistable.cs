﻿using FineWoodworkingBasic.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;

namespace FineWoodworkingBasic.Model
{
    public abstract class Persistable
    {
        protected ResultMessage saveMessage = new ResultMessage(ResultMessage.ResultMessageType.Success, "");
        protected ResultMessage populateMessage = new ResultMessage(ResultMessage.ResultMessageType.Success, "");

        protected abstract bool IsNewObject();

        protected abstract void SetupCommandForInsert(QC.SqlCommand command);

        protected abstract void SetupCommandForUpdate(QC.SqlCommand command);

        protected abstract void SetAutogeneratedIDFromInsert(SqlGuid genID);

        protected abstract ResultMessage GetErrorMessageForSave(Exception excep);

        protected abstract ResultMessage GetResultMessageForSave();
        public virtual void Save()
        {
            try
            {
                using (QC.SqlConnection connection = new QC.SqlConnection(Utilities.GetConnectionString()))
                {
                    connection.Open();

                    using (var command = new QC.SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = DT.CommandType.Text;

                        if (IsNewObject())
                        {
                            SetupCommandForInsert(command);

                            SqlGuid genID = (SqlGuid)command.ExecuteScalar();
                            SetAutogeneratedIDFromInsert(genID);
                            saveMessage = GetResultMessageForSave();
                        }
                        else
                        {
                            SetupCommandForUpdate(command);
                            command.ExecuteNonQuery();
                            saveMessage = GetResultMessageForSave();
                        }
                    }

                }
            }
            catch (Exception Ex) {
                // Call to logging facility to log exception - record source info (class, method), date and time, log message
                saveMessage = GetErrorMessageForSave(Ex);
            }
            
        }


        protected void PopulateHelper(Dictionary<string, Object> populateData)
        {
            try
            {
                // Code from website 
                using (QC.SqlConnection connection = new QC.SqlConnection(Utilities.GetConnectionString()))
                {
                    connection.Open();

                    using (var command = new QC.SqlCommand())
                    {

                        command.Connection = connection;
                        command.CommandType = DT.CommandType.Text;
                        ConstructPopulateQueryCommand(populateData, command);

                        QC.SqlDataReader reader = command.ExecuteReader();

                        ProcessPopulateQueryResult(reader);
                        populateMessage = GetResultMessageForPopulate();

                    }
                }
            }
            catch (Exception Ex)
            {
                // Call to logging facility to log exception - record source info (class, method), date and time, log message
                populateMessage = GetErrorMessageForPopulate(Ex);
            }
            
        }

        protected abstract void ConstructPopulateQueryCommand(Dictionary<string, Object> populateData, QC.SqlCommand command);

        protected abstract void ProcessPopulateQueryResult(QC.SqlDataReader reader);

        protected abstract ResultMessage GetErrorMessageForPopulate(Exception excep);

        protected abstract ResultMessage GetResultMessageForPopulate();

        public abstract bool IsPopulated();
        public ResultMessage RetrieveSaveMessage() { return saveMessage; }

        public ResultMessage RetrievePopulateMessage() { return populateMessage; }  
    }
}
