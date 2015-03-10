using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.Testing.MSTest.Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace F2F.Testing.MSTest.IntegrationTests
{
	[TestClass]
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

		[TestMethod]
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