using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DeafX.Richter.Common.Identity
{
    class LiteDbIdentityStore<T> : IUserStore<T>, IUserPasswordStore<T>, IUserClaimStore<T> where T:class, IUser<string>
    {
        public Task AddClaimAsync(T user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(T user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(T user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T user)
        {
            throw new NotImplementedException();
        }
    }
}
