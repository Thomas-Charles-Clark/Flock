using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Flock.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class FlockUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<FlockUser, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //public int UserID { get; set; }

        //public byte LanguageID { get; set; }

        //public short CountryID { get; set; }

        //public DateTime CreatedDT { get; set; }

        //public DateTime? DeletedDT { get; set; }

        //public long? ColonyID { get; set; }

        //public bool Trade { get; set; }

        //public DateTime? HardBounceDT { get; set; }

        //public DateTime? UnsubscribedDT { get; set; }

        //public short UserSourceID { get; set; }

        //public DateTime? UserSyncDT { get; set; }

        //public string Name { get; set; }

        //public string OldPassword { get; set; }

        //public bool PasswordUpgraded { get; set; }

        public string DefaultOrderTaker { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<FlockUser>
    {
        public ApplicationDbContext()
            : base("FlockConnectionString", throwIfV1Schema: false)
        {
            //this.OnModelCreating(new DbModelBuilder());
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            // heeeeell no
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());

            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<FlockUser>().ToTable("User");
            modelBuilder.Entity<IdentityRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserRole>().ToTable("User_UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
        }
    }
}