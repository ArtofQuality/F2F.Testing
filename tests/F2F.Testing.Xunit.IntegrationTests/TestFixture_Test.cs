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
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_Test : TestFixture
	{
		public TestFixture_Test()
		{
		}

#if NUNIT
		[Test]
#endif
#if XUNIT || XUNIT2
		[Fact]
#endif
#if MSTEST
		[TestMethod, Ignore]
#endif
		public void When_Fixture_Is_Disposed__Should_Call_Dispose()
		{
			// only exists to have a test case that is executed from this class, so its Dispose will get called
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (!disposing)
			{
				true.Should().BeFalse(because: "Dispose(true) wasn't called.");
			}
		}
	}
}