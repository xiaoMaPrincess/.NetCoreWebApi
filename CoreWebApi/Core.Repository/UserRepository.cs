using Core.Common.Dapper;
using Core.Common.Helper;
using Core.IRepository;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    /// <summary>
    /// 用户数据层实现
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _db;
        public UserRepository(DbContext db)
        {
            _db = db;
        }
        public async Task<LoginResult> UserRegister(LoginVM loginVM)
        {
            LoginResult loginResult = new LoginResult();
            loginVM.PasswordMD5 = MD5Helper.GetMD5String(loginVM.Password);
            string sql = @"INSERT INTO systemuser ( ID, CreateTime, Creator, ITCode, PASSWORD, Email, NAME, Sex,IsValid )
                            VALUES  (UUID(), NOW(), 'sys', @ITCode, @PasswordMD5, @Email, @Name, 0, 1)";
            //DbContext.ExecuteScalarAsync()
            await DbContext.ExecuteAsync(sql, loginVM);
            loginResult.Reason = "成功";
            return loginResult;

        }
    }
}
