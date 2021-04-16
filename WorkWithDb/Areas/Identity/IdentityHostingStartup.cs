using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkWithDb.Areas.Identity.Data;
using WorkWithDb.Data;

[assembly: HostingStartup(typeof(WorkWithDb.Areas.Identity.IdentityHostingStartup))]
namespace WorkWithDb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WorkWithDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WorkWithDbContextConnection")));

                services.AddDefaultIdentity<WorkWithDbUser>(options => {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 1;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                    .AddEntityFrameworkStores<WorkWithDbContext>();
            });
        }
    }
}