using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.Testing.Xunit.FakeItEasy;
using FluentAssertions;
using Xunit;

namespace F2F.Testing.Xunit.IntegrationTests
{
	public class TestFixture_FileSandboxFeature_Test : TestFixture
	{
		public TestFixture_FileSandboxFeature_Test()
		{
			Register(new FileSandboxFeature());
			//Register(new AutoMockFeature());
		}

		[Fact]
		public void When_Requesting_Sandbox_Fixture__Should_Not_Be_Null()
		{
			// Act
			var f = Use<FileSandboxFeature>();

			// Assert
			f.Sandbox.Should().NotBeNull();
		}
	}
}