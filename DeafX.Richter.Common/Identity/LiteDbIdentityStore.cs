using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using LiteDB;
using System.Linq;

namespace DeafX.Richter.Common.Identity
{
    public class LiteDbIdentityStore<T> : IUserStore<T>, IUserPasswordStore<T>, IUserClaimStore<T> where T:class, IUser<string>
    {
        private class InternalUser
        {
            public string Id { get; set; }

            public T User { get; set; }

            public string PasswordHash { get; set; }

            public IList<Claim> Claims { get; set; }
        }


        private string _storagePath;

        private const string USERS_COLLECTION = "USERS";

        public LiteDbIdentityStore(string storagePath)
        {

        }

        #region User methods

        public Task CreateAsync(T user)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<InternalUser>(USERS_COLLECTION);

                collection.Insert(new InternalUser()
                {
                    User = user,
                    Claims = new List<Claim>(),
                });

                collection.EnsureIndex(o => o.User.UserName);
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(T user)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<InternalUser>(USERS_COLLECTION);

                collection.Delete(u => user.Id.Equals(u.User.Id));
            }

            return Task.CompletedTask;
        }

        public Task<T> FindByIdAsync(string userId)
        {
            var usr = GetInternalUser(userId);

            return Task.FromResult(usr.User);
        }

        public Task<T> FindByNameAsync(string userName)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<InternalUser>(USERS_COLLECTION);

                var usr = collection.FindOne(u => userName.Equals(u.User.UserName));

                return Task.FromResult(usr.User);
            }
        }


        public Task UpdateAsync(T user)
        {
            return UpdateInternalUser(user, (usr) =>
            {
                usr.User = user;
            });
        }

        #endregion


        #region Claims methods

        public Task AddClaimAsync(T user, Claim claim)
        {
            return UpdateInternalUser(user, (usr) =>
            {
                usr.Claims.Add(claim);
            });
        }

        public Task<IList<Claim>> GetClaimsAsync(T user)
        {
            var usr = GetInternalUser(user.Id);

            return Task.FromResult(usr.Claims);
        }

        public Task RemoveClaimAsync(T user, Claim claim)
        {
            return UpdateInternalUser(user, (usr) =>
            {
                var claimToRemove = usr.Claims.First(c => c.Type == claim.Type);
                usr.Claims.Remove(claimToRemove);
            });
        }

        #endregion

        #region Password Methods

        public Task<string> GetPasswordHashAsync(T user)
        {
            var usr = GetInternalUser(user.Id);

            return Task.FromResult(usr.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(T user)
        {
            var usr = GetInternalUser(user.Id);

            return Task.FromResult(!string.IsNullOrWhiteSpace(usr.PasswordHash));
        }

        public Task SetPasswordHashAsync(T user, string passwordHash)
        {
            return UpdateInternalUser(user, (usr) =>
            {
                usr.PasswordHash = passwordHash;
            });
        }

        #endregion

        public void Dispose() { }

        private Task UpdateInternalUser(T user, Action<InternalUser> userAction)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<InternalUser>(USERS_COLLECTION);

                var usr = collection.FindOne(u => user.Id.Equals(u.User.Id));

                userAction(usr);

                collection.Update(usr);

                return Task.CompletedTask;
            }
        }

        private InternalUser GetInternalUser(string userId)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<InternalUser>(USERS_COLLECTION);

                return collection.FindOne(u => userId.Equals(u.User.Id));
            }
        }

    }
}
