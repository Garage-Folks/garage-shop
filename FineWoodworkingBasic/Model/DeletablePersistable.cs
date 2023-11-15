using FineWoodworkingBasic.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DT = System.Data;
using QC = Microsoft.Data.SqlClient;

namespace FineWoodworkingBasic.Model
{

    public abstract class DeletablePersistable : Persistable
    {
        protected abstract void SetupCommandForDelete(QC.SqlCommand command);

        protected abstract ResultMessage GetErrorMessageForDelete(Exception excep);

        protected abstract ResultMessage GetResultMessageForDelete();

        protected ResultMessage deleteMessage = new ResultMessage(ResultMessage.ResultMessageType.Success, "");
        public virtual void Delete()
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

                        SetupCommandForDelete(command);
                        command.ExecuteNonQuery();
                        deleteMessage = GetResultMessageForDelete();

                    }

                }
            }
            catch (Exception Ex)
            {
                // Call to logging facility to log exception - record source info (class, method), date and time, log message
                deleteMessage = GetErrorMessageForDelete(Ex);
            }



        }

    }
}
