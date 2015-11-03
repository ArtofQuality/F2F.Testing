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

#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_FeatureDisposal_Test : TestFixture
	{
		private class Disposable : IDisposable
		{
			private readonly Action _onDispose;

			public Disposable(Action onDispose)
			{
				_onDispose = onDispose;
			}

			public void Dispose()
			{
				_onDispose();
			}
		}

		public TestFixture_FeatureDisposal_Test()
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
		public void When_Fixture_Is_Disposed__Should_Dispose_Features_In_Reverse_Order_Of_Registration()
		{
			var feature1 = new Disposable(() => feature1DisposedTicks = DateTimeOffset.Now.Ticks);
			var feature2 = new Disposable(() => feature2DisposedTicks = DateTimeOffset.Now.Ticks);

			this.Register(feature1);
			this.Register(feature2);
		}

        long feature1DisposedTicks = 0, feature2DisposedTicks = 0;

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (!disposing)
			{
				true.Should().BeFalse(because: "Dispose(true) wasn't called.");
			}
			else
			{
				// feature2 should be disposed before feature1
				feature2DisposedTicks.Should().BeLessThan(feature1DisposedTicks);
			}
		}
	}
}