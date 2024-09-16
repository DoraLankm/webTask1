using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace WebApplication3.Models
{
    public class ProductContext : DbContext
    {
       

        public DbSet<ProductStorage> ProductStorages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Storage> Storages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=localhost;Username=postgres;Password=example;Database=Product");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Product>(entity =>
            {
                // Указываем имя таблицы для сущности Product
                entity.ToTable("Products");

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
                    .IsRequired();                    // Поле обязательно для заполнения

                // Настройка связи с сущностью Category (многие к одному)
                entity.HasOne(x => x.Category)         // Один продукт связан с одной категорией
                    .WithMany(c => c.Products)         // Одна категория может содержать много продуктов
                    .HasForeignKey(x => x.CategoryId)  // Связываем по полю CategoryId (внешний ключ)
                    .HasConstraintName("FK_Product_Category")  // Устанавливаем имя ограничения для внешнего ключа
                    .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление — если удаляем категорию, удаляются все связанные продукты

                // Связь с сущностью ProductStorage (один ко многим)
                entity.HasMany(p => p.ProductStorage)
                    .WithOne(ps => ps.Product)         // Один ProductStorage связан с одним продуктом
                    .HasForeignKey(ps => ps.ProductId) // Связываем по полю ProductId
                    .HasConstraintName("FK_Product_ProductStorage")
                    .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление ProductStorage при удалении продукта
            });

            modelBuilder.Entity<Category>(entity =>
            {
                // Указываем имя таблицы для сущности Category
                entity.ToTable("Categories");

                // Настраиваем первичный ключ
                entity.HasKey(x => x.Id).HasName("PK_Category");

                // Создаём уникальный индекс на поле Name
                entity.HasIndex(x => x.Name).IsUnique().HasDatabaseName("IX_Category_Name");

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
                entity.HasOne(s => s.Product)          // Один склад имеет один продукт
                    .WithOne()                         // Продукт связан с одним складом
                    .HasForeignKey<Storage>(s => s.ProductId)  // Связываем по полю ProductId
                    .HasConstraintName("FK_Storage_Product")   // Устанавливаем имя ограничения для внешнего ключа
                    .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление — если удаляем продукт, удаляется и склад
            });

            modelBuilder.Entity<ProductStorage>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.StorageId }); 

                entity.HasOne(e => e.Product)
                      .WithMany(p => p.ProductStorage)
                      .HasForeignKey(e => e.ProductId);

                entity.HasOne(e => e.Storage)
                      .WithMany(s => s.ProductStorage)
                      .HasForeignKey(e => e.StorageId);
            });


        }
    }
}
