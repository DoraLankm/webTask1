using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace WebApplication3.Models
{
    public class ProductContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Storage> Storages { get; set; }


        public ProductContext(DbContextOptions<ProductContext> options)
           : base(options)
        {
        }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Product>(entity =>
            {
                // Указываем имя таблицы для сущности Product
                entity.ToTable("Products")
                .HasIndex(p => new { p.Name, p.CategoryId })
                .IsUnique();

                // Настраиваем первичный ключ
                entity.HasKey(x => x.Id).HasName("Product_ID");

                // Создаём уникальный индекс на поле Name
                entity.HasIndex(x => x.Name).IsUnique();

                // Настройка для свойства Name
                entity.Property(e => e.Name)
                    .HasColumnName("ProductName")     // Имя колонки в базе данных
                    .HasMaxLength(255)                // Ограничение длины строки
                    .IsRequired();                    // Поле обязательно для заполнения

                // Настройка для свойства Description
                entity.Property(e => e.Description)
                    .HasColumnName("Description")     // Имя колонки в базе данных
                    .HasMaxLength(255)                // Ограничение длины строки
                    .IsRequired();                    // Поле обязательно для заполнения

                // Настройка для свойства Cost (цена)
                entity.Property(e => e.Cost)
                    .HasColumnName("Cost")            // Имя колонки в базе данных
                    .IsRequired();      // Поле обязательно для заполнения
                

                // Настройка связи с сущностью Category (многие к одному)
                entity.HasOne(x => x.Category)         // Один продукт связан с одной категорией
                    .WithMany(c => c.Products);         // Одна категория может содержать много продуктов

                
            });

            modelBuilder.Entity<Category>(entity =>
            {
                // Указываем имя таблицы для сущности Category
                entity.ToTable("Categories");

                // Настраиваем первичный ключ
                entity.HasKey(x => x.Id).HasName("PK_Category");

                // Создаём уникальный индекс на поле Name
                entity.HasIndex(x => x.Name).IsUnique().HasDatabaseName("Category_Name");

                // Настройка для свойства Name
                entity.Property(e => e.Name)
                    .HasColumnName("CategoryName")     // Имя колонки в базе данных
                    .HasMaxLength(255)                 // Ограничение длины строки
                    .IsRequired();                     // Поле обязательно для заполнения

                // Настройка для свойства Description
                entity.Property(e => e.Description)
                    .HasColumnName("Description")      // Имя колонки в базе данных
                    .HasMaxLength(255)                 // Ограничение длины строки
                    .IsRequired();                     // Поле обязательно для заполнения
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                // Указываем имя таблицы для сущности Storage
                entity.ToTable("Storages");

                // Настраиваем первичный ключ
                entity.HasKey(x => x.Id).HasName("PK_Storage");

                // Настройка для свойства Name
                entity.Property(e => e.Name)
                    .HasColumnName("StorageName")      // Имя колонки в базе данных
                    .HasMaxLength(255)                 // Ограничение длины строки
                    .IsRequired();                     // Поле обязательно для заполнения

                // Настройка для свойства Description
                entity.Property(e => e.Description)
                    .HasColumnName("Description")      // Имя колонки в базе данных
                    .HasMaxLength(255)                 // Ограничение длины строки
                    .IsRequired();                     // Поле обязательно для заполнения

                // Связь "один к одному" с Product
                entity.HasOne(s => s.Product)
                    .WithMany(p => p.Storages);

            });
        }
    }
}
