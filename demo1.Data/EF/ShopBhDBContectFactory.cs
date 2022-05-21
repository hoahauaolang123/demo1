using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo1.Data.EF
{
    public class ShopBhDBContectFactory : IDesignTimeDbContextFactory<DBShopContext>
    {
        public DBShopContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                .Build();

            var connectionString = configuration.GetConnectionString("ShopBHDbContext");

            var optionsBuilder = new DbContextOptionsBuilder<DBShopContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DBShopContext(optionsBuilder.Options);
        }
    }
}
