using IniParser.Model;
using IniParser;
using System.Xml.Linq;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;

namespace FineWoodworkingBasic.Util
{
    public static class Utilities
    {

        private static string ConnectionString = "";

        private static string ServerName = "localhost";
        private static string DbName = "test";
        private static string LoginName = "";
        private static string Password = "";

        public static void SetServerName(string serverName)
        {
            ServerName = serverName;
        }

        public static void SetDBName(string dbName)
        {
            DbName = dbName;
        }

        public static void SetLoginName(string loginName)
        {
            LoginName = loginName;
        }

        public static void SetPassword(string password)
        {
            Password = password;
        }

        public static void SetConnectionString()
        {
            /* ConnectionString = "Server=tcp:" + serverName +
             "Database=" + dbName + ";User ID=" + loginNm + ";" +
             "Password=" + passwd + ";Encrypt=True;" +
             "TrustServerCertificate=False;Connection Timeout=30;"; */

            /*ConnectionString = "Server = " + serverName + "; Database = " + dbName + "; User Id = " +
                loginNm + "; Password = " + passwd + ";"; */

            ConnectionString = "Server = " + ServerName + "; Database = " + DbName + "; Encrypt=False; Trusted_Connection = True;";
        }

        public static string GetConnectionString()
        {
            return ConnectionString;
        }

        public static void EstablishConnection(string filePath)
        {
            // Need to read the file which will have info like this
            // serverName=...
            // dbName=...
            // loginName=...
            // password=...

            // and set the above static variables from this file

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            SetServerName(data["Server"]["ServerName"]);
            SetDBName(data["Server"]["DBName"]);
            SetLoginName(data["Server"]["LoginName"]);
            SetPassword(data["Server"]["Password"]);

            SetConnectionString();

        }

        public static string Encrypt(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (!name.IsNullOrEmpty())
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
                return value.ToString();
            }
            return null;
        }
    }
}
