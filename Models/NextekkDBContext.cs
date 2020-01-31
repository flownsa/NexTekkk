﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Nextekk.Models
{
    public class NextekkDBContext: IdentityDbContext<Employee, Role, string>
    // IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        // IdentityRoleClaim<string>, IdentityUserToken<string>>
    {

        public NextekkDBContext(DbContextOptions<NextekkDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public DbSet<IdentityUserClaim<string>> IdentityUserClaims { get; set; }  
        public virtual DbSet<Role> Role { get; set; }
        // public virtual DbSet<OtherInfo> OtherInfo { get; set; }


         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Role>(entity => entity.ToTable(name:"Role"));
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name:"EmployeeRole"));
            
            // modelBuilder.Ignore <IdentityUserLogin<string>>();
//          passes type Employee to create table with
            modelBuilder.Entity<Employee>(entity => 
            {
                entity.ToTable(name:"Employee");
                 // To unbind unused derived identity properties
                // entity.Ignore(ignore => ignore.Id);
                // entity.Property(e => e.Id)
                // .entity.ValueGeneratedNever();
                entity.Ignore(ignore => ignore.ConcurrencyStamp);
                // entity.Ignore(ignore => ignore.NormalizedUserName);
                entity.Ignore(ignore => ignore.EmailConfirmed); 
                entity.Ignore(ignore => ignore.NormalizedEmail);
                // entity.Ignore(ignore => ignore.PasswordHash);
                // entity.Ignore(ignore => ignore.SecurityStamp);
                entity.Ignore(ignore => ignore.PhoneNumberConfirmed);
                entity.Ignore(ignore => ignore.TwoFactorEnabled);
                entity.Ignore(ignore => ignore.LockoutEnd);
                entity.Ignore(ignore => ignore.LockoutEnabled);
                entity.Ignore(ignore => ignore.AccessFailedCount);
    
                // entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Dob).HasColumnType("date");
                // entity.HasMany(e => e.Claims).WithOne().HasForeignKey(fk => fk.UserId).IsRequired();
                // entity.HasMany(e => e.EmployeeRole).WithOne().HasForeignKey(fk => fk.EmployeeId);
                // entity.HasMany(e => e.Role).WithOne().HasForeignKey(fk => fk.EmployeeId);
                // entity.HasOne(e => e.OtherInfo).WithOne().HasForeignKey(fk => fk.EmployeeId).IsRequired();

            
            });

            modelBuilder.Entity<OtherInfo>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateJoined).HasColumnType("date");

                entity.Property(e => e.DatePromoted).HasColumnType("date");

                entity.Property(e => e.MaxEdu).HasMaxLength(50).IsUnicode(false);

                entity.Property(e => e.Position).HasMaxLength(50).IsUnicode(false);

                entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.School).HasMaxLength(250).IsUnicode(false);

                entity.Property(e => e.TimeServed).HasColumnType("date");

                // entity.HasOne(e => e.EmployeeId).WithOne(p => p.Employee).HasForeignKey<OtherInfo>(d => d.EmployeeId)
                    // .OnDelete(DeleteBehavior.ClientSetNull)
                    // .HasConstraintName("FK_OtherInfoEmployee");
            });
                
            // modelBuilder.Entity<EmployeeRole>(entity =>
            // {
            //     entity.ToTable(name:"EmployeeRole");
            //     //
            // });
                  
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                // optionsBuilder.UseSqlServer("Server=localhost;Database=NextekkDB;User Id=SA;Password=+ot@l123./");
            }
        }

       
        
        


    }
}