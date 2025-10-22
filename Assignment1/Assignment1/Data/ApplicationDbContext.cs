using Assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        
        // ************** TABLES **************  //
        
        public DbSet<Category> Categories => Set<Category>();
        
        public DbSet<Event> Events => Set<Event>();

        public DbSet<Purchase> Purchases => Set<Purchase>();

        public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();



        // ************** RELATIONSHIPS **************  //
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        
        {
            
            base.OnModelCreating(modelBuilder);

            // EVENTS //
            modelBuilder.Entity<Event>()
                
                .HasOne(e => e.Category)
                
                .WithMany(c => c.Events)
                
                .HasForeignKey(e => e.CategoryId)
                
                .OnDelete(DeleteBehavior.Cascade);
            
            
            //  PURCHASE ITEM //
            modelBuilder.Entity<PurchaseItem>()
                
                .HasKey(pi => new { pi.PurchaseId, pi.EventId });
            
            
            // PURCHASE (1) //
            modelBuilder.Entity<PurchaseItem>()

                .HasOne(pi => pi.Purchase)

                .WithMany(p => p.Items)

                .HasForeignKey(pi => pi.PurchaseId)

                .OnDelete(DeleteBehavior.Cascade);
            
            
            // EVENT (1) //
            modelBuilder.Entity<PurchaseItem>()

                .HasOne(pi => pi.EventInfo)

                .WithMany(e => e.PurchaseItems)

                .HasForeignKey(pi => pi.EventId)

                .OnDelete(DeleteBehavior.Cascade);

        }
        
        

    

        }
    
}
    
    

