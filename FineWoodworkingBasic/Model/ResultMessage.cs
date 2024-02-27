using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineWoodworkingBasic.Model
{
    public class ResultMessage
    {
        public enum ResultMessageType
        {
            Success,
            Error,
            Information
        }

        public ResultMessageType MessageType { get; set; }
        public string Message { get; set; }

        public ResultMessage(ResultMessageType messageType, string message)
        {
            MessageType = messageType;
            Message = message;
        }

        public Boolean Equals(ResultMessage other)
        {
            Boolean retVal = false;
            if ((MessageType == other.MessageType) && (Message == other.Message)) { retVal = true; }
            return retVal;
        }

    }
}
