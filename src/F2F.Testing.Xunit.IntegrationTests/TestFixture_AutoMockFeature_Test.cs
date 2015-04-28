using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Ploeh.AutoFixture;

#if NUNIT
using NUnit.Framework;
using F2F.Testing.NUnit.FakeItEasy;
namespace F2F.Testing.NUnit.IntegrationTests
#endif

#if XUNIT
using Xunit;
using F2F.Testing.Xunit.FakeItEasy;
namespace F2F.Testing.Xunit.IntegrationTests
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using F2F.Testing.MSTest.Moq;
namespace F2F.Testing.MSTest.IntegrationTests
#endif

{
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
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

#if NUNIT
		[Test]
#endif
#if XUNIT
		[Fact]
#endif
#if MSTEST
		[TestMethod]
#endif
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