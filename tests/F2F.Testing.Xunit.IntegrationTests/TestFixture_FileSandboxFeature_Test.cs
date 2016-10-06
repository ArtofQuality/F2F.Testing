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

	/// <summary>
	/// The class tests the integration between TestFixture and FileSandboxFeature
	/// </summary>
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_FileSandboxFeature_Test : TestFixture
	{
		/// <summary>
		/// Constructor. Registers the File Sandbox Feature
		/// </summary>
		public TestFixture_FileSandboxFeature_Test()
		{
			Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
		}

		/// <summary>
		/// Tests if the SandboxFeature instance is returned when it's requested
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
		public void When_Requesting_Sandbox_Feature__Should_Not_Be_Null()
		{
			// Act
			var sut = Use<FileSandboxFeature>();

			// Assert
			sut.Sandbox.Should().NotBeNull();
		}

		/// <summary>
		/// The test ensures that when a file is provided by the FileSandboxFeature, this file exists 
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
		public void When_Providing_File__Should_Exist()
		{
			// Arrange
			var sut = Use<FileSandboxFeature>();

			// Act
			var file = sut.Sandbox.ProvideFile("testdata/TextFile1.txt");

			// Assert
			File.Exists(file).Should().BeTrue();
		}

		/// <summary>
		/// The test ensures that when a file is provided by the FileSandboxFeature, this file contains the expected content
		/// </summary>
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

		/// <summary>
		/// Tests that the FileSandboxFeature it is set to null once disposed
		/// </summary>
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

		/// <summary>
		/// Tests that the Database file has been deleted once the FileSandboxFeature has been disposed
		/// </summary>
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