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

#if XUNIT
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
	public class TestFixture_LocalDbFeature_Test : TestFixture
	{
		public TestFixture_LocalDbFeature_Test()
		{
			Register(new LocalDbFeature());
		}

#if NUNIT
		[Test]
#endif
#if XUNIT
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

#if NUNIT
		[Test]
#endif
#if XUNIT
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