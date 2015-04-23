using System;
using System.Collections.Generic;
using System.Text;
using F2F.Sandbox;

namespace F2F.Testing.Xunit.Sandbox
{
	/// <summary>
	/// Extension methods for TestFixture.
	/// </summary>
	public static class TestFixtureExtensions
	{
		/// <summary>
		/// Wrapper for Use&lt;FileSandboxFeature&gt;().Fixture.
		/// </summary>
		/// <param name="fixture">The test fixture</param>
		/// <returns>The file sandbox instance.</returns>
		public static IFileSandbox Sandbox(this TestFixture fixture)
		{
			return fixture.Use<FileSandboxFeature>().Sandbox;
		}
	}
}