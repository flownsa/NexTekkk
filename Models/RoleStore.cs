// using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Nextekk.Models;
using Microsoft.EntityFrameworkCore;


namespace Nextekk.Models
{ 
    public class RoleStore : IRoleStore<Role>
    {
        private readonly NextekkDBContext db;
      
        public RoleStore(NextekkDBContext db) 
        {
            this.db = db;
        }

        public void Dispose()
        {
            Dispose(true);
        } 
        protected virtual void Dispose(bool disposing)
        {
            // release database connection

            if (disposing)
            {
                db?.Dispose();
            }
        }
 
        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            db.Add(role);

            await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            db.Entry(role).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await db.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken) 
        {
            db.Remove(role);

            int i = await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());

        }
        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken) 
        {

             if (int.TryParse(roleId, out int id))
            {
                return await db.Role.FindAsync(id);
            }
            else
            {
                return await Task.FromResult((Role)null);
            }
        }
        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
         return await db.Role.AsAsyncEnumerable().SingleOrDefault(p => p.Name.Equals(normalizedRoleName), cancellationToken);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);        
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }
        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }
        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }
        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }
        

    }

}