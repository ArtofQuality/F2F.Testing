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
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_LocalDbContextFeature_Test : TestFixture
	{
		public class CustomerContext : DbContext
		{
			public CustomerContext(string nameOrConnectionString)
				: base(nameOrConnectionString)
			{
			}

			public DbSet<Customer> Customers { get; set; }
		}

		public class Customer
		{
			[Key]
			public string Name { get; set; }
		}

		public TestFixture_LocalDbContextFeature_Test()
		{
			Register(new LocalDbContextFeature());
		}

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
			var sut = Use<LocalDbContextFeature>();

			// Act
			var ctx = sut.CreateContext<CustomerContext>();

			// Assert
			ctx.Should().NotBeNull();
		}

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
			var sut = Use<LocalDbContextFeature>();
			sut.CreateContext<CustomerContext>();

			// Act
			sut.Dispose();

			// Assert
			File.Exists(Use<LocalDbContextFeature>().DatabaseFile).Should().BeFalse();
		}
	}
}