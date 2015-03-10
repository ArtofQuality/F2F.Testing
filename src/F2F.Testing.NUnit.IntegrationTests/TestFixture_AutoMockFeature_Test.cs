using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.Testing.NUnit.FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace F2F.Testing.NUnit.IntegrationTests
{
	[TestFixture]
	public class TestFixture_AutoMockFeature_Test : TestFixture
	{
		public interface ISample
		{
			string Name { get; }
		}

		public TestFixture_AutoMockFeature_Test()
		{
			Register(new AutoMockFeature());
		}

		[Test]
		public void When_Create_Interface_With_Fixture__Should_Not_Be_Null()
		{
			// Arrange
			var sut = Use<AutoMockFeature>();

			// Act
			var f = sut.Fixture.Create<ISample>();

			// Assert
			f.Should().NotBeNull();
		}
	}
}