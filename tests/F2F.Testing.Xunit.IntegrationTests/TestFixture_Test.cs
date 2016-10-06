using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Ploeh.AutoFixture;
using System.Threading;

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
	/// Tests for the TestFixture
	/// </summary>
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_Test : TestFixture
	{
        private class SomeFeature : Feature
        {
            public bool OnUseCalled = false;

            public override void OnUse()
            {
                OnUseCalled = true;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestFixture_Test()
		{
		}

        /// <summary>
        /// Tests that the feature's OnUse() method is correctly called
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
        public void When_Feature_Is_Used__Should_Call_OnUse_On_Feature()
        {
            var feature = new SomeFeature();

            this.Register(feature);

            this.Use<SomeFeature>();

            feature.OnUseCalled.Should().BeTrue();
        }

        /// <summary>
        /// Tests whether the Fixture calls to the Dispose method when is Disposed
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
		public void When_Fixture_Is_Disposed__Should_Call_Dispose()
		{
			// only exists to have a test case that is executed from this class, so its Dispose will get called
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (!disposing)
			{
				true.Should().BeFalse(because: "Dispose(true) wasn't called.");
			}
		}
	}

	/// <summary>
	/// Tests that the TestFixture disposes all the registered features properly
	/// </summary>
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_FeatureDisposal_Test : TestFixture
	{
		private class DisposableFeature : Feature, IDisposable
		{
			private readonly Action _onDispose;

			public DisposableFeature(Action onDispose)
			{
				_onDispose = onDispose;
			}

			public void Dispose()
			{
				_onDispose();
			}

            public bool OnUseCalled = false;

			public override void OnUse()
			{
                OnUseCalled = true;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public TestFixture_FeatureDisposal_Test()
		{
		}

        /// <summary>
        /// Tests that the features are disposed in reverse order of registration when the Fixture is diposed
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
		public void When_Fixture_Is_Disposed__Should_Dispose_Features_In_Reverse_Order_Of_Registration()
		{
			var feature1 = new DisposableFeature(() =>
			{
				Thread.Sleep(1);    // force thread to sleep, so code doesn't execute on the same tick
				feature1DisposedTicks = DateTimeOffset.Now.Ticks;
			});
			var feature2 = new DisposableFeature(() =>
			{
				feature2DisposedTicks = DateTimeOffset.Now.Ticks;
			});

			this.Register(feature1);
			this.Register(feature2);
		}

		long feature1DisposedTicks = 0, feature2DisposedTicks = 0;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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