using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;

namespace F2F.Testing.Xunit.FakeItEasy
{
	/// <summary>
	/// Extension methods for TestFixture.
	/// </summary>
	public static class TestFixtureExtensions
	{
		/// <summary>
		/// Wrapper for Use&lt;AutoMockFeature&gt;().Fixture.
		/// </summary>
		/// <param name="fixture">The test fixture</param>
		/// <returns>The AutoFixture instance.</returns>
		public static IFixture Fixture(this TestFixture fixture)
		{
			return fixture.Use<AutoMockFeature>().Fixture;
		}
	}
}