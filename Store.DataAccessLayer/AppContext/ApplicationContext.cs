using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Entities;
using System;

namespace Store.DataAccess.AppContext
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<Guid>, Guid,
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        public DbSet<PrintingEdition> PrintingEditions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AuthorInPrintingEdition>()
                .HasKey(table => new { table.AuthorId, table.PrintingEditionId });

            builder.Entity<AuthorInPrintingEdition>()
                .HasOne(ap => ap.PrintingEdition)
                .WithMany(pe => pe.AuthorInPrintingEditions)
                .HasForeignKey(ap => ap.PrintingEditionId);

            builder.Entity<AuthorInPrintingEdition>()
                .HasOne(ap => ap.Author)
                .WithMany(author => author.AuthorInPrintingEditions)
                .HasForeignKey(ap => ap.AuthorId);

            base.OnModelCreating(builder);
        }
    }
}
