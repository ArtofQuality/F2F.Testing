using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.Testing.Sandbox;
using NUnit.Framework;
using FluentAssertions;

namespace F2F.Testing.NUnit.IntegrationTests
{
	[TestFixture]
	public class TestFixture_FileSandboxFeature_Test : TestFixture
	{
		public TestFixture_FileSandboxFeature_Test()
		{
			Register(new FileSandboxFeature(new ResourceFileLocator(typeof(TestFixture_FileSandboxFeature_Test))));
		}

		[Test]
		public void When_Requesting_Sandbox_Fixture__Should_Not_Be_Null()
		{
			// Act
			var f = Use<FileSandboxFeature>();

			// Assert
			Assert.IsNotNull(f.Sandbox);
		}

		[Test]
		public void When_Requesting_File__Then_Provide_Should_Succeed()
		{
			// Arrange
			var s = Use<FileSandboxFeature>();

			// Act
			s.Sandbox.ProvideFile("testdata/TextFile1.txt").Should().NotBeNull();
		}
	}
}