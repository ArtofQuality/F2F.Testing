using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;

#if NUNIT
using F2F.Testing.NUnit;
namespace F2F.Testing.NUnit.FakeItEasy
#endif

#if XUNIT || XUNIT2
using F2F.Testing.Xunit;
namespace F2F.Testing.Xunit.FakeItEasy
#endif
{
	/// <summary>
	/// Provides AutoFixture initialized with FakeItEasy.
	/// </summary>
	public class AutoMockFeature : Feature
	{
		private IFixture _fixture;

		/// <summary>
		/// The fixture.
		/// </summary>
		public IFixture Fixture
		{
			get { return _fixture; }
		}

		/// <summary>
		/// Initialize AutoFixture with FakeItEasy.
		/// </summary>
		public AutoMockFeature()
		{
			_fixture = new Fixture()
				.Customize(new AutoFakeItEasyCustomization());
		}
	}
}