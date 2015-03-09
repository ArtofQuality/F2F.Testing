using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using F2F.Testing.Sandbox;
using Xunit;

namespace F2F.Testing.Xunit.IntegrationTests
{
	public class FileSandboxFeature_ResourceFileLocator_Test : FileSandboxFeature
	{
		/// <summary>Initializes the test fixture with the given type.</summary>
		/// <param name="caller">The type of the caller.</param>
		public FileSandboxFeature_ResourceFileLocator_Test()
			: base(new ResourceFileLocator(typeof(FileSandboxFeature_ResourceFileLocator_Test)))
		{
		}

		[Fact]
		public void When_Providing_File__Should_Exist()
		{
			// Arrange
			const string file = "testdata/TextFile1.txt";

			// Act
			string filePath = Sandbox.ProvideFile(file);

			// Assert
			Assert.True(File.Exists(filePath));
		}

		[Fact()]
		public void When_Providing_File__Should_Contain_Expected_Content()
		{
			// Arrange
			const string file = "testdata/TextFile1.txt";
			const string expectedContent = "huhuu";

			// Act
			string filePath = Sandbox.ProvideFile(file);

			// Assert
			Assert.Equal(expectedContent, File.ReadAllText(filePath));
		}
	}
}