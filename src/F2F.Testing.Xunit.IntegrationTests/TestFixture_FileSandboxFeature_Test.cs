using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using F2F.Sandbox;

#if NUNIT
using NUnit.Framework;
using F2F.Testing.NUnit.Sandbox;
namespace F2F.Testing.NUnit.IntegrationTests
#endif

#if XUNIT || XUNIT2
using Xunit;
using F2F.Testing.Xunit.Sandbox;
namespace F2F.Testing.Xunit.IntegrationTests
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using F2F.Testing.MSTest.Sandbox;
namespace F2F.Testing.MSTest.IntegrationTests
#endif

{
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_FileSandboxFeature_Test : TestFixture
	{
		public TestFixture_FileSandboxFeature_Test()
		{
			Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
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
		public void When_Requesting_Sandbox_Fixture__Should_Not_Be_Null()
		{
			// Act
			var sut = Use<FileSandboxFeature>();

			// Assert
			sut.Sandbox.Should().NotBeNull();
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
		public void When_Providing_File__Should_Exist()
		{
			// Arrange
			var sut = Use<FileSandboxFeature>();

			// Act
			var file = sut.Sandbox.ProvideFile("testdata/TextFile1.txt");

			// Assert
			File.Exists(file).Should().BeTrue();
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
		public void When_Providing_File__Should_Contain_Expected_Content()
		{
			// Arrange
			var fileName = "testdata/TextFile1.txt";
			var expectedContent = "Hello World!";
			var sut = Use<FileSandboxFeature>();

			// Act
			var file = sut.Sandbox.ProvideFile(fileName);

			// Assert
			File.ReadAllText(file).Should().Be(expectedContent);
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
		public void Dispose_ShouldSetSandboxToNull()
		{
			// Arrange
			var sut = Use<FileSandboxFeature>();

			// Act
			sut.Dispose();

			// Assert
			sut.Sandbox.Should().BeNull();
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
		public void Dispose_ShouldDeleteDatabaseFile()
		{
			// Arrange
			var sut = Use<FileSandboxFeature>();
			var directory = sut.Sandbox.Directory;

			// Act
			sut.Dispose();

			// Assert
			Directory.Exists(directory).Should().BeFalse();
		}
	}
}