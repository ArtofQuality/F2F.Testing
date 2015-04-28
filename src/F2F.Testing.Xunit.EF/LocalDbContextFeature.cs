using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

#if NUNIT
using NUnit.Framework;
namespace F2F.Testing.NUnit.EF
#endif

#if XUNIT
namespace F2F.Testing.Xunit.EF
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace F2F.Testing.MSTest.EF
#endif

{
	/// <summary>
	/// A factory for creating a EntityFramework context on a temporary local db.
	/// </summary>
	public class LocalDbContextFeature : LocalDbFeature, IDisposable
	{
		private readonly IList<DbContext> _contexts = new List<DbContext>();

		private class DatabaseInitializer<TContext> : DropCreateDatabaseAlways<TContext>
			where TContext : DbContext
		{
			protected override void Seed(TContext context)
			{
				context.SaveChanges();
			}
		}

#if NUNIT

		/// <summary>Tear down the database.</summary>
		[TearDown]
		public void NUnit_TearDownLocalDb()
		{
			Dispose();
		}

#endif

#if MSTEST

		/// <summary>Tear down the database.</summary>
		[TestCleanup]
		public void MSTest_TearDownLocalDb()
		{
			Dispose();
		}

#endif

		/// <summary>
		/// Create an entity context on temporary file.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <returns>The entity context.</returns>
		public TContext CreateContext<TContext>()
			where TContext : DbContext
		{
			var context = (TContext)Activator.CreateInstance(typeof(TContext), ConnectionString);

			if (context == null)
				throw new ArgumentException(String.Format("could not create context for type {0}", typeof(TContext).Name));

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

				_contexts.Clear();
			}

			base.Dispose();
		}
	}
}