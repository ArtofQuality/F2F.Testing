using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using F2F.Testing.Sandbox;

#if NUNIT
namespace F2F.Testing.NUnit.EF
#endif

#if XUNIT
namespace F2F.Testing.Xunit.EF
#endif

#if MSTEST
namespace F2F.Testing.MSTest.EF
#endif

{
	public class LocalDbContextFeature : IDisposable
	{
		private readonly IFileSandbox _sandbox = new FileSandbox(new EmptyFileLocator());

		private class DatabaseInitializer<TContext> : DropCreateDatabaseAlways<TContext>
			where TContext : DbContext
		{
			protected override void Seed(TContext context)
			{
				context.SaveChanges();
			}
		}

		public TContext CreateContext<TContext>()
			where TContext : DbContext
		{
			var context = (TContext)Activator.CreateInstance(typeof(TContext), GetConnectionString());

			var initializer = new DatabaseInitializer<TContext>();
			Database.SetInitializer(initializer);
			context.Database.Initialize(true);

			return context;
		}

		private string GetConnectionString()
		{
			var dbfile = _sandbox.GetTempFile("mdf");
			return String.Format(@"Data Source=(localdb)\v11.0;AttachDbFileName={0};Integrated Security=True;Connect Timeout=5", dbfile);
		}

		public void Dispose()
		{
			_sandbox.Dispose();
		}
	}
}
