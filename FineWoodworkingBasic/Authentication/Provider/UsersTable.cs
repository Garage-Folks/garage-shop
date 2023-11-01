using FineWoodworkingBasic.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Data;

namespace FineWoodworkingBasic.Authentication.Provider
{
    public class UsersTable
    {
        private readonly SqlConnection _connection;
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
                    " VALUES (@Username, @Password);";
                command.CommandText = sql;

                parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);  // Fix Type and Length 
                parameter.Value = user.UserName;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@Password", SqlDbType.NVarChar, 100);  // Fix Type and Length 
                parameter.Value = user.Password;
                command.Parameters.Add(parameter);

                rows = await command.ExecuteNonQueryAsync();
            }

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
            string sql = "SELECT * " +
                        "FROM dbo.CustomUser " +
                        "WHERE UserName = @UserName;";

            throw new NotImplementedException();
        }
    }
}
