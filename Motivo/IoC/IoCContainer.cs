using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		/// <summary>
		/// The Configuration manager for the application
		/// </summary>
		public static IConfiguration Configuration { get; set; }

	}
}
