using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using F2F.Testing.MSTest.EF;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace F2F.Testing.MSTest.IntegrationTests
{
	[TestClass]
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

		[TestMethod]
		public void When_Creating_Context__Should_Not_Be_Null()
		{
			// Arrange
			var sut = Use<LocalDbContextFeature>();

			// Act
			var ctx = sut.CreateContext<CustomerContext>();

			// Assert
			ctx.Should().NotBeNull();
		}
	}
}