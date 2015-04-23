using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using F2F.Testing.Xunit.EF;
using F2F.Testing.Xunit.Sandbox;
using FluentAssertions;
using Xunit;

namespace F2F.Testing.Xunit.IntegrationTests
{
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
			Register(new FileSandboxFeature());
			Register(new LocalDbContextFeature(Use<FileSandboxFeature>().Sandbox));
		}

		[Fact]
		public void When_Creating_Context__Should_Not_Be_Null()
		{
			// Arrange
			var sut = Use<LocalDbContextFeature>();

			// Act
			var ctx = sut.CreateContext<CustomerContext>();

			// Assert
			ctx.Should().NotBeNull();
		}

		[Fact]
		public void Dispose_ShouldDeleteDirectory()
		{
			// Arrange
			var sut = Use<LocalDbContextFeature>();
			var ctx = sut.CreateContext<CustomerContext>();

			// Act
			ctx.Dispose();
			sut.Dispose();

			// Assert
			Directory.Exists(this.Sandbox().Directory).Should().BeFalse();
		}
	}
}