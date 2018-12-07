using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Finance.Models
{


    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=1gb_developmentdb", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Incoming> Incomings { get; set; }
        public DbSet<Outgoing> Outgoings { get; set; }
      

        public DbSet<IncomingCategory> IncomingCategorys { get; set; }
       
        public DbSet<OutgoingsCategory> OutgoingsCategorys { get; set; }
        public DbSet<OutgoingsType> OutgoingsTypes { get; set; }
     
        public DbSet<WayOfPayment> WayOfPayments { get; set; }
        
        public DbSet<PaymentStatement> PaymentStatements { get; set; }
        public DbSet<RemovedCounterparty> RemovedCounterpartys { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<PaymentsDocument> PaymentsDocuments { get; set; }
    }
}