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

#if XUNIT || XUNIT2
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

	/// <summary>
	/// Tests Create Fixture method
	/// </summary>
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_AutoMockFeature_Test : TestFixture
	{
		/// <summary>
		/// Sample interface
		/// </summary>
		public interface ISample
		{
			/// <summary>
			/// Sample property
			/// </summary>
			string Name { get; }
		}

		/// <summary>
		/// Contructor, registers automockFeature
		/// </summary>
		public TestFixture_AutoMockFeature_Test()
		{
			Register(new AutoMockFeature());
		}

		/// <summary>
		/// Tests Fixture Create method with a given interface
		/// </summary>
#if NUNIT
		[Test]
#endif
#if XUNIT || XUNIT2
		[Fact]
#endif
#if MSTEST
		[TestMethod, Ignore]
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