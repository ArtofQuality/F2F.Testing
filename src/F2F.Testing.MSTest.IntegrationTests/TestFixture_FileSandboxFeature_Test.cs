using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.Testing.Sandbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace F2F.Testing.MSTest.IntegrationTests
{
	[TestClass]
	public class TestFixture_FileSandboxFeature_Test : TestFixture
	{
		public TestFixture_FileSandboxFeature_Test()
		{
			Register(new FileSandboxFeature(new ResourceFileLocator(typeof(TestFixture_FileSandboxFeature_Test))));
		}

		[TestMethod]
		public void When_Requesting_Sandbox_Fixture__Should_Not_Be_Null()
		{
			// Act
			var f = Use<FileSandboxFeature>();

			// Assert
			Assert.IsNotNull(f.Sandbox);
		}

		[TestMethod]
		public void When_Requesting_File__Then_Provide_Should_Succeed()
		{
			// Arrange
			var s = Use<FileSandboxFeature>();

			// Act
			s.Sandbox.ProvideFile("testdata/TextFile1.txt").Should().NotBeNull();
		}
	}
}
