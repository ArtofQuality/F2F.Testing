using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using F2F.Testing.NUnit.EF;
using FluentAssertions;
using NUnit.Framework;

namespace F2F.Testing.NUnit.IntegrationTests
{
	[TestFixture]
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

		[Test]
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