using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace F2F.Testing.MSTest.Moq
{
	/// <summary>
	/// Provides AutoFixture initialized with Moq.
	/// </summary>
	public class AutoMockFeature
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
		/// Initialize AutoFixture with Moq.
		/// </summary>
		public AutoMockFeature()
		{
			_fixture = new Fixture()
				.Customize(new AutoMoqCustomization());
		}
	}
}
