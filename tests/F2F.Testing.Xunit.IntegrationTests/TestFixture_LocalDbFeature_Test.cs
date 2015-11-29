using System;
using System.Collections.Generic;
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
	/// The class tests the integration between TestFixture and LocalDbFeature
	/// </summary>
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_LocalDbFeature_Test : TestFixture
	{
		/// <summary>
		/// Constructor, registers LocalDbFeature
		/// </summary>
		public TestFixture_LocalDbFeature_Test()
		{
			Register(new LocalDbFeature());
		}

		/// <summary>
		/// Tests whether the database file exists once the LocalDbFeature is provided
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
		public void DatabaseFile_ShouldNotBeNull()
		{
			// Arrange
			var sut = Use<LocalDbFeature>();

			// Act && Assert
			sut.DatabaseFile.Should().NotBeNull();
		}

		/// <summary>
		/// Tests that the connectionString is not null when the LocalDbFeature is provided
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
		public void ConnectionString_ShouldNotBeNull()
		{
			// Arrange
			var sut = Use<LocalDbFeature>();

			// Act && Assert
			sut.ConnectionString.Should().NotBeNull();
		}
	}
}