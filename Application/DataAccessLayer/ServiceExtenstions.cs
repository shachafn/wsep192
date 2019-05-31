using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public static class ServiceExtenstions
    {
        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["Data Source=DESKTOP-3MH7VAJ\\SQLEXPRESS;Initial Catalog=WSEP192;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"];
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString));
        }
    }
}
