using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;

#if NUNIT
using NUnit.Framework;
using F2F.Testing.NUnit.EF;
namespace F2F.Testing.NUnit.IntegrationTests
#endif

#if XUNIT || XUNIT2
using Xunit;
using F2F.Testing.Xunit.EF;
namespace F2F.Testing.Xunit.IntegrationTests
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using F2F.Testing.MSTest.EF;
namespace F2F.Testing.MSTest.IntegrationTests
#endif

{

	/// <summary>
	/// The class tests the integration between TestFixture and LocalDbContextFeature
	/// </summary>
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_LocalDbContextFeature_Test : TestFixture
	{

		/// <summary>
		/// DbContext sample. Customer DbContext
		/// </summary>
		public class CustomerContext : DbContext
		{

			/// <summary>
			/// Constructor. Calls the base class constructor
			/// </summary>
			/// <param name="nameOrConnectionString">Name or ConnectionString</param>
			public CustomerContext(string nameOrConnectionString)
				: base(nameOrConnectionString)
			{
			}

			/// <summary>
			/// Sample DbSet. Customers
			/// </summary>
			public DbSet<Customer> Customers { get; set; }
		}

		/// <summary>
		/// Sample Entity Customer
		/// </summary>
		public class Customer
		{
			/// <summary>
			/// Sample property. Name
			/// </summary>
			[Key]
			public string Name { get; set; }
		}

		/// <summary>
		/// Constructor. Registers the LocalDbContextFeature
		/// </summary>
		public TestFixture_LocalDbContextFeature_Test()
		{
			var feature = new LocalDbContextFeature();
			

			Register(feature);
		}

		private LocalDbContextFeature CreateSut()
		{
			var sut = Use<LocalDbContextFeature>();
			sut.ConnectionString = String.Format(@"Data Source=(localdb)\ProjectsV12;AttachDbFileName={0};Integrated Security=True;Connect Timeout=5", sut.DatabaseFile);

			return sut;
		}

		/// <summary>
		/// Tests that the Context it is set to an instance once it's created
		/// </summary>
#if NUNIT
		[Test]
#endif
#if XUNIT || XUNIT2
		[Fact]
#endif
#if MSTEST
		[TestMethod]
#endif
		public void When_Creating_Context__Should_Not_Be_Null()
		{
			// Arrange
			var sut = CreateSut();

			// Act
			var ctx = sut.CreateContext<CustomerContext>();

			// Assert
			ctx.Should().NotBeNull();
		}

		/// <summary>
		/// Tests that the Database file has been deleted once the FileSandboxFeature has been disposed
		/// </summary>
#if NUNIT
		[Test]
#endif
#if XUNIT || XUNIT2
		[Fact]
#endif
#if MSTEST
		[TestMethod]
#endif
		public void Dispose_ShouldDeleteDatabaseFile()
		{
			// Arrange
			var sut = CreateSut();
			sut.CreateContext<CustomerContext>();

			// Act
			sut.Dispose();

			// Assert
			File.Exists(Use<LocalDbContextFeature>().DatabaseFile).Should().BeFalse();
		}
	}
}