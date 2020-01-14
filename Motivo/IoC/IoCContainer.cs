using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Motivo.Data;

namespace Motivo.IoC
{
    public static class IoC
    {
        public static MotivoDbContext MotivoDbContext => IoCContainer.Provider.GetService<MotivoDbContext>();
    }

    //The Dependency Injection Container making use of .NetCore ServiceProvider
    public static class IoCContainer
    {
        public static ServiceProvider Provider { get; set; }

        // The Configuration manager for the application
        public static IConfiguration Configuration { get; set; }

    }
}
