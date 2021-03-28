using ActionBlazor.PushNotifications.Server.Models;
using ActionBlazor.PushNotifications.Shared;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActionBlazor.PushNotifications.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<NotificationUsers> NotificationUsers { get; set; }
        public DbSet<SubscriptionUsers> SubscriptionUsers { get; set; }
        public DbSet<NotificationSubscription> NotificationSubscription { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotificationUsers>().HasKey(pst => new { pst.NotificationId });
            modelBuilder.Entity<SubscriptionUsers>().HasKey(pst => new { pst.SubscriptionId });
            modelBuilder.Entity<NotificationSubscription>().HasKey(pst => new { pst.NotificationSubscriptionId });

            // Configuring a many-to-many special -> topping relationship that is friendly for serialization
            //modelBuilder.Entity<NotificationSubscription>().HasKey(pst => new { pst.SubsctiptionId, pst.NotificationId });
            //modelBuilder.Entity<NotificationSubscription>().HasOne<SubscriptionUsers>().WithMany(ps => ps.SubscriptionId);
            //modelBuilder.Entity<NotificationSubscription>().HasOne<NotificationUsers>(pst => pst.NotificationId).WithMany();

            // Inline the Lat-Long pairs in Order rather than having a FK to another table
            //modelBuilder.Entity<>().OwnsOne(o => o.);
        }

    }
}
