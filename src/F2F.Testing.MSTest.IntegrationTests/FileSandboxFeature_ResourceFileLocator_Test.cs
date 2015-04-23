using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using F2F.Sandbox;
using F2F.Testing.MSTest.Sandbox;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace F2F.Testing.MSTest.IntegrationTests
{
	[TestClass]
	public class FileSandboxFeature_ResourceFileLocator_Test : FileSandboxFeature
	{
		public FileSandboxFeature_ResourceFileLocator_Test()
			: base(new ResourceFileLocator(typeof(FileSandboxFeature_ResourceFileLocator_Test)))
		{
		}

		[TestMethod]
		public void When_Providing_File__Should_Exist()
		{
			// Arrange
			var fileName = "testdata/TextFile1.txt";

			// Act
			var absoluteFile = Sandbox.ProvideFile(fileName);

			// Assert
			File.Exists(absoluteFile).Should().BeTrue();
		}

		[TestMethod]
		public void When_Providing_File__Should_Contain_Expected_Content()
		{
			// Arrange
			var fileName = "testdata/TextFile1.txt";
			var expectedContent = "Hello World!";

			// Act
			var absoluteFile = Sandbox.ProvideFile(fileName);

			// Assert
			File.ReadAllText(absoluteFile).Should().Be(expectedContent);
		}
	}
}