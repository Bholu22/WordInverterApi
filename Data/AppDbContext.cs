using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordInverterApi.Models;

namespace WordInverterApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<InversionRecord> InversionRecords => Set<InversionRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InversionRecord>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.RequestSentence)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.ResponseSentence)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();
            });
        }
    }
}