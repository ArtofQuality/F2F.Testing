using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using F2F.Sandbox;

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
	/// <summary>
	/// A factory for creating a EntityFramework context on a temporary local db.
	/// </summary>
	public class LocalDbContextFeature : LocalDbFeature, IDisposable
	{
		private IList<DbContext> _contexts = new List<DbContext>();

		private class DatabaseInitializer<TContext> : DropCreateDatabaseAlways<TContext>
			where TContext : DbContext
		{
			protected override void Seed(TContext context)
			{
				context.SaveChanges();
			}
		}

		/// <summary>
		/// Create an entity context on temporary file.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <returns>The entity context.</returns>
		public TContext CreateContext<TContext>()
			where TContext : DbContext
		{
			var context = (TContext)Activator.CreateInstance(typeof(TContext), ConnectionString);

			var initializer = new DatabaseInitializer<TContext>();
			Database.SetInitializer(initializer);
			context.Database.Initialize(true);

			_contexts.Add(context);

			return context;
		}

		/// <summary>
		/// Dispose all created contexts.
		/// </summary>
		public override void Dispose()
		{
			if (_contexts != null)
			{
				foreach (var ctx in _contexts)
				{
					ctx.Database.Delete();
					ctx.Dispose();
				}

				_contexts = null;
			}
		}
	}
}