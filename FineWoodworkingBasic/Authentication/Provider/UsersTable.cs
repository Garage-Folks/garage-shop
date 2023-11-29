using FineWoodworkingBasic.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Data;
using MudBlazor;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static MudBlazor.CategoryTypes;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using System.Data.Common;

namespace FineWoodworkingBasic.Authentication.Provider
{
    public class UsersTable
    {
        private readonly SqlConnection _connection;

        public UsersTable() 
        {
            _connection = new SqlConnection();
            _connection.ConnectionString = Utilities.GetConnectionString();
        }
        public UsersTable(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            _connection.Open();

            int rows = 0;

            using (var command = new SqlCommand())
            {
                command.Connection = _connection;
                command.CommandType = CommandType.Text;

                SqlParameter parameter;

                string sql = "INSERT INTO AuthorizedUser (Username, Password) " +
                    " OUTPUT INSERTED.ID " +
                    " VALUES (@UserName, @Password);";
                command.CommandText = sql;

                parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);  // Fix Type and Length 
                parameter.Value = user.UserName;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@Password", SqlDbType.NVarChar, 100);  // Fix Type and Length 
                parameter.Value = user.PasswordHash;
                command.Parameters.Add(parameter);

                rows = await command.ExecuteNonQueryAsync();
            }

            _connection.Close();

            if (rows > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.UserName}." });
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            string sql = "DELETE FROM dbo.CustomUser WHERE Id = @Id";
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            string sql = "SELECT * " +
                        "FROM dbo.CustomUsers " +
                        "WHERE Id = @Id;";

            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            _connection.Open();

            SqlDataReader reader;
            int ID;
            string UN;
            string PH;
            string R;

            using (var command = new SqlCommand())
            {
                command.Connection = _connection;
                command.CommandType = CommandType.Text;

                SqlParameter parameter;

                string sql = @"SELECT * FROM AuthorizedUser WHERE Username = @UserName;";
                command.CommandText = sql;

                parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);  // Fix Type and Length 
                parameter.Value = userName;
                command.Parameters.Add(parameter);

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ID = reader.GetInt32(0);
                    UN = reader.GetString(1);
                    PH = reader.GetString(2);
                    R = reader.GetString(3);

                    _connection.Close();

                    return new ApplicationUser { Id = ID, UserName = UN, PasswordHash = PH, Role = R };
                }

                _connection.Close();
                return null;
            }
        }
        public async Task<bool> CreateUsersTableAsync()
        {
            _connection.Open();

            using (var command = new SqlCommand())
            {
                command.Connection = _connection;
                command.CommandType = CommandType.Text;

                string sql = "CREATE TABLE dbo.AuthorizedUser(" +
                            "ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY," +
                            "Username NVARCHAR(50) NOT NULL," +
                            "Password NVARCHAR(MAX) NOT NULL, " +
                            "Role NVARCHAR(50) NOT NULL DEFAULT 'User');";
                command.CommandText = sql;

                try { await command.ExecuteNonQueryAsync(); }
                catch { _connection.Close(); return false; }
            }

            _connection.Close();

            return true;
        }

        public async Task<bool> CreateRolesTableAsync()
        {
            _connection.Open();

            using (var command = new SqlCommand())
            {
                command.Connection = _connection;
                command.CommandType = CommandType.Text;

                string sql = "CREATE TABLE dbo.Roles(" +
                            "ID INT NOT NULL PRIMARY KEY," +
                            "Name NVARCHAR(50) NOT NULL );" +
                            "INSERT INTO [dbo].[Roles] VALUES (1, 'User');" +
                            "INSERT INTO [dbo].[Roles] VALUES (2, 'Admin');";
                command.CommandText = sql;

                try { await command.ExecuteNonQueryAsync(); }
                catch { _connection.Close(); return false; }
            }

            _connection.Close();

            return true;
        }

        public async Task<bool> SetUserRole(string username, string role)
        {
            _connection.Open();

            using (var command = new SqlCommand())
            {
                command.Connection = _connection;
                command.CommandType = CommandType.Text;

                SqlParameter parameter;

                string sql = "UPDATE dbo.AuthorizedUser SET Role = @Role WHERE Username = @UserName";
                command.CommandText = sql;

                parameter = new SqlParameter("@Role", SqlDbType.NVarChar, 50);
                parameter.Value = role;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
                parameter.Value = username;
                command.Parameters.Add(parameter);

                try { await command.ExecuteNonQueryAsync(); }
                catch { _connection.Close(); return false; }
            }

            _connection.Close();

            return true;
        }
    }
}
