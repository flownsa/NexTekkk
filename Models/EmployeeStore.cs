using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;



namespace Nextekk.Models
{
    public class EmployeeStore : IdentityUserClaim<string>, IUserStore<Employee>,IUserPasswordStore<Employee>,
    IUserRoleStore<Employee>// IUserClaimStore<Employee> // Join Employee on Role


    //  public class EmployeeStore : IdentityUserClaim<string>, IUserStore<Employee>,IUserPasswordStore<Employee>,
    // IUserRoleStore<Employee>, IUserClaimStore<Employee> 

{
    // Implementations3
    // Context class propertys
    private readonly NextekkDBContext db;
    
        public EmployeeStore (NextekkDBContext db) : base()
        {
            
            this.db = db;
        }
        public void Dispose()
        // garbage collector
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

        // Userstore
        public Task<string> GetUserIdAsync(Employee employee, CancellationToken cancellationToken)
        {
            // var query = db.Employees.Where(e => e == employee).SingleOrDefault();
            
            return Task.FromResult(employee.Id);
        }

        public Task<string> GetUserNameAsync(Employee employee, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // ThrowIfDisposed();
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var query = db.Employees.Select(e => e.Id == employee.UserName).SingleOrDefault().ToString();

                return Task.FromResult(query);
        }

        public Task SetUserNameAsync(Employee employee, string userName, CancellationToken cancellationToken)
        {
            
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }   
            var query = db.Employees.Where(emp => emp.UserName == employee.UserName).SingleOrDefault(); //match by username
            //  db.Employees.UserName = userName;
             query.UserName = userName;
             db.SaveChanges();
             return Task.CompletedTask;
        }


        public Task<string> GetNormalizedUserNameAsync(Employee employee, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            var uname = db.Employees.Where(e => e.Id == employee.Id).SingleOrDefault().ToString();
            return Task.FromResult(uname);
        }


         public Task SetNormalizedUserNameAsync(Employee employee, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            if (normalizedName == null)
            {
                throw new ArgumentNullException(nameof(normalizedName));
            }
            var query = db.Employees.Where(e => e.Id == employee.Id).SingleOrDefault();
            query.NormalizedUserName = normalizedName;
            db.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(Employee employee, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (employee == null) 
            {
                throw new ArgumentNullException(nameof(employee));
            }

            db.Add(employee);
 
            await db.SaveChangesAsync(cancellationToken);
 
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> UpdateAsync(Employee employee, CancellationToken cancellationToken)
        {
            if (employee == null) 
            {
                throw new ArgumentNullException(nameof(employee));
            }
            db.Add(employee);
 
            await db.SaveChangesAsync(cancellationToken);
 
            return await Task.FromResult(IdentityResult.Success);
        }

         public async Task<IdentityResult> DeleteAsync(Employee employee, CancellationToken cancellationToken)
        {
            db.Remove(employee);
             
            int i = await db.SaveChangesAsync(cancellationToken); // return code
 
            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }   

         public async Task<Employee> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await db.Employees.AsAsyncEnumerable().SingleOrDefault(p => p.UserName.Equals(normalizedUserName, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public async Task<Employee> FindByIdAsync(string emloyeeId, CancellationToken cancellationToken)
        {
            if (int.TryParse(emloyeeId, out int Id)) // validating and changing index key type
            {
                // return await db.Employees.FindAsync(Id, cancellationToken);
                return await db.Employees.FindAsync(new object[] {Id}, cancellationToken);
            }

            else
            {
                return await Task.FromResult((Employee) null);
            }

        }


        // EmployeeRole
        public Task AddToRoleAsync(Employee employee, string rolename, CancellationToken cancellationToken)
        {
            // add user to role
          
            // inline statement to catch rolename validity and assign its ID
            var roles = db.Role.FirstOrDefault(r => r.Name == rolename);
            var roleId = roles.Id;


            string employeeId = employee.Id;
            
            var employeerole = new IdentityUserRole<string>()
            { 
                UserId = employee.Id, 
                RoleId = roleId
            };
            
            // Add new UserRole to IdentityUser table
            db.UserRoles.Add(employeerole);
            
            db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return Task.FromResult(db.SaveChanges());
        }

        public async Task RemoveFromRoleAsync(Employee employee, string rolename, CancellationToken cancellationToken)
        {
            
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var matchrole = from urole in db.UserRoles where urole.UserId == employee.Id select urole;
            foreach (var user_role in matchrole)
            {
                db.UserRoles.Remove(user_role);
            }
            db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await db.SaveChangesAsync(cancellationToken);
        }

        public Task<IList<string>> GetRolesAsync(Employee employee, CancellationToken cancellationToken)
        {            
            // if employee is null catch an exception
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            
            var query = from urole in db.UserRoles
            join emp in db.Employees on urole.UserId equals emp.Id 
            where emp.Id == employee.Id
            select urole.RoleId; // Collection of roleids corresponding to employee
            

            IEnumerable<string> filteredUID = query.ToList();   // filtered RoleIds for employee 

            List<string> bag = new List<string>();   // bag to prepare and return list of rolenames

            foreach ( var roleid in filteredUID )
            {
                var x = db.Role.Where(role => role.Id == roleid).Select(r => r.Name); // Collection of roleids  
                bag.Add(x.ToString());
            }
            
            return Task.FromResult<IList<string>>(bag);

        }
            
//      check employee against role
        public virtual Task<bool> IsInRoleAsync(Employee employee, string rolename, CancellationToken cancellationToken)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (string.IsNullOrEmpty(rolename))
            {
                throw new ArgumentNullException(nameof(rolename));
            }

            var roleid = (db.Role.Where( x => x.Name == rolename ).SingleOrDefault()).Id;
            
            return Task.FromResult(db.UserRoles.Any( x => x.UserId == employee.Id && x.RoleId == roleid));

        }

        public async Task<IList<Employee>> GetUsersInRoleAsync(string rolename, CancellationToken cancellationToken)
        {
            if (rolename == null)
            {
                throw new ArgumentNullException(nameof(rolename));
            }

            // get employee ids into list collection
            List<string> employeeids = db.UserRoles.Where(urole => urole.RoleId == "").Select(b => b.UserId).Distinct().ToList();
            
            // get these scan employeeid List against Employee table in db
            List<Employee> employeeInrole = db.Employees.Where(a => employeeids.All(c => c == a.Id)).ToList();

            return await Task.FromResult(employeeInrole);
            
        }

        // Passwordhasher

        public Task SetPasswordHashAsync(Employee employee, string passwordHash, CancellationToken cancellationToken)
        {
            var query = db.Employees.Where(e => e.UserName == employee.UserName).FirstOrDefault();
            

            return Task.FromResult(query.Password = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(Employee employee, CancellationToken cancellationToken)
        {
            return Task.FromResult(employee.Password);
        }

        public Task<bool> HasPasswordAsync(Employee employee, CancellationToken cancellationToken)
        {
            var query = db.Employees.Where(emp => emp.Id == employee.Id).FirstOrDefault(); 
            return Task.FromResult(!string.IsNullOrWhiteSpace(query.Password));
        }

        // Employee Claims

        // ClaimStore Interface
        public Task AddClaimsAsync(Employee employee, IdentityUserClaim<string> claims, CancellationToken cancellationToken)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            } 
            
            var query = db.IdentityUserClaims.Add(claims);

            return Task.FromResult(0);
        }  
        // Gets a list of Claims belonging to specified employee
        public Task<IList<Claim>> GetClaimsAsync(Employee employee, CancellationToken cancellationToken)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            return Task.FromResult<IList<Claim>>(db.IdentityUserClaims.Select(claim => new Claim(claim.ClaimType, claim.ClaimValue)).ToList());
        }
        
    
    public Task RemoveClaimsAsync(Employee employee, IdentityUserClaim<string> claims, CancellationToken cancellationToken)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            var query = db.IdentityUserClaims.Where(iuc => iuc.UserId == employee.Id && iuc.ClaimType == claims.ClaimType); // from Microsoft.EntityFrameworkCore.
            db.Remove(query); // Not sure if this is rightly done
            db.SaveChanges();   

            return Task.FromResult(0);
        }

    public Task ReplaceClaimAsync(Employee employee, Claim claim, Claim newclaim, CancellationToken cancellationToken)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }

        if (newclaim == null)
            {
                throw new ArgumentNullException(nameof(newclaim));
            }
            
            var match = db.UserClaims.Where(uc => uc.UserId.Equals(employee.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
            foreach (var m in match.ToList())
            {
                m.ClaimValue = newclaim.Value;
                m.ClaimType = newclaim.Type;
            }

            return Task.FromResult(0);
    }

    public async Task<IList<Employee>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
//          DBSET query to match employee with Claims UserId property and then extract that user
            var query = from userclaims in db.IdentityUserClaims
                        join employee in db.Employees on userclaims.UserId equals employee.Id
                        where userclaims.ClaimValue == claim.Value
                        && userclaims.ClaimType == claim.Type
                        select employee;

            return await query.ToListAsync(cancellationToken);

        }

 
}


}