using IBM.Products.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace IBM.Products
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Helpers.Seed();
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}