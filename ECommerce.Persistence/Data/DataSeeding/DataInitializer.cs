using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeeding
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {

            try
            {
                var HasBrands   = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes    = await _dbContext.ProductTypes.AnyAsync();
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasDeliveryMethod = await _dbContext.Set<DeliveryMethod>().AnyAsync();

                if (HasBrands && HasTypes && HasProducts && HasDeliveryMethod)
                    return;


                if (!HasBrands)
                {

                   await SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);

                }

                if (!HasTypes)
                {

                    await SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);
                }
                await _dbContext.SaveChangesAsync();

                if (!HasProducts)
                {
                    await SeedDataFromJson<Product, int>("products.json", _dbContext.Products);
                }

                if(!HasDeliveryMethod)
                {
                    await SeedDataFromJson<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While Reading Data From json {ex}");

            }


        }


        private async Task SeedDataFromJson<T, Tkey>(string fileName, DbSet<T> dbSet) where T : BaseEntity<Tkey>
        {
            // D:\Assignement and demo c#\solutions\My Own Project API\E-CommerceSolution\ECommerce.Persistence\Data\DataSeeding\JsonFiles\brands.json

            var filePath = @"..\ECommerce.Persistence\Data\DataSeeding\JsonFiles\" + fileName;

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Json File Not Found", filePath);
            }

            try
            {
                // var data = File.ReadAllText(filePath); // Read All Json File And Return It Into String Variable , This is Not Recommended

                var dataStream = File.OpenRead(filePath);

                var data = await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data is not null)
                {
                  await dbSet.AddRangeAsync(data);
                }

            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine($"Error While Reading Data From json {ex}");
                }

            }


        }
    }
}
