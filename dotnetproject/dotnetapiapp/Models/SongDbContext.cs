using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStoreDBFirst.Models;

public class SongDbContext : DbContext
{
    public SongDbContext()
    {
    }

    public SongDbContext(DbContextOptions<SongDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Song> Songs { get; set; }

}
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<Book>(entity =>
//         {
//             entity.HasKey(e => e.Id).HasName("PK__Book__3213E83F97675A18");

//             entity.ToTable("Book");

//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Author)
//                 .HasMaxLength(100)
//                 .IsUnicode(false)
//                 .HasColumnName("author");
//             entity.Property(e => e.Price)
//                 .HasColumnType("decimal(18, 2)")
//                 .HasColumnName("price");
//             entity.Property(e => e.Quantity).HasColumnName("quantity");
//             entity.Property(e => e.Title)
//                 .HasMaxLength(100)
//                 .IsUnicode(false)
//                 .HasColumnName("title");
//         });

//         OnModelCreatingPartial(modelBuilder);
//     }

//     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// }
