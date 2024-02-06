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
using System.Data.SqlTypes;

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

                string sql = "INSERT INTO AuthorizedUser (Username, Password, Role) " +
                    " OUTPUT INSERTED.ID " +
                    " VALUES (@UserName, @Password, @Role);";
                command.CommandText = sql;

                parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);  // Fix Type and Length 
                parameter.Value = user.UserName;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@Password", SqlDbType.NVarChar);  // Fix Type and Length 
                parameter.Value = user.PasswordHash;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@Role", SqlDbType.NVarChar);  // Fix Type and Length 
                parameter.Value = "User";
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
            SqlGuid ID;
            string UN;
            string PH;
            string? EM;
            string? N;
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
                    ID = reader.GetSqlGuid(0);
                    UN = reader.GetString(1);
                    PH = reader.GetString(2);
                    EM = reader.IsDBNull(3) ? null : reader.GetString(3);
                    N = reader.IsDBNull(4) ? null : reader.GetString(4);
                    R = reader.IsDBNull(5) ? "User" : reader.GetString(5);

                    _connection.Close();

                    return new ApplicationUser { Id = ID, UserName = UN, PasswordHash = PH, Email = EM, Notes = N, Role = R };
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
                            "ID uniqueidentifier NOT NULL IDENTITY(1,1) PRIMARY KEY," +
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
                            "ID uniqueidentifier NOT NULL PRIMARY KEY," +
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
