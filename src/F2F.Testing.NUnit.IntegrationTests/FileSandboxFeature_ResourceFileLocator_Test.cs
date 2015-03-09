using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using F2F.Testing.Sandbox;
using NUnit.Framework;

namespace F2F.Testing.NUnit.IntegrationTests
{
	[TestFixture]
	public class FileSandboxFeature_ResourceFileLocator_Test : FileSandboxFeature
	{
		/// <summary>Initializes the test fixture with the given type.</summary>
		/// <param name="caller">The type of the caller.</param>
		public FileSandboxFeature_ResourceFileLocator_Test()
			: base(new ResourceFileLocator(typeof(FileSandboxFeature_ResourceFileLocator_Test)))
		{
		}

		[TestCase("testdata/TextFile1.txt")]
		public void When_Providing_File__Should_Exist(string file)
		{
			// Arrange

			// Act
			string filePath = Sandbox.ProvideFile(file);

			// Assert
			Assert.True(File.Exists(filePath));
		}

		[TestCase("testdata/TextFile1.txt", "Hello World!")]
		public void When_Providing_File__Should_Contain_Expected_Content(string file, string expectedContent)
		{
			// Arrange

			// Act
			string filePath = Sandbox.ProvideFile(file);

			// Assert
			Assert.AreEqual(expectedContent, File.ReadAllText(filePath));
		}
	}
}