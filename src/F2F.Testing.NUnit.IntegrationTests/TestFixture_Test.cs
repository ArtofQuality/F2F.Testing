using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.Testing.Sandbox;
using NUnit.Framework;
using FluentAssertions;

namespace F2F.Testing.NUnit.IntegrationTests
{
	[TestFixture]
	[Ignore("no sql server available")]
	public class TestFixture_Test : TestFixture
	{
		public TestFixture_Test()
		{
			Register(new FileSandboxFeature(new ResourceFileLocator(typeof(TestFixture_FileSandboxFeature_Test))));
			Register(new SqlServerFeature("Data Source=192.168.0.139;User ID=test;Password=test"));
			Register(new AppConfigFeature());
		}

		[Test]
		public void When_Requesting_SqlServer_Fixture__Should_Succeed()
		{
			// Act
			var f = Use<SqlServerFeature>();

			// Assert
			Assert.IsNotNull(f.Database);
		}

		[Test]
		public void When_Requesting_SqlServer_Fixture__Then_Import_Should_Succeed()
		{
			// Arrange
			var s = Use<FileSandboxFeature>();
			var f = Use<SqlServerFeature>();

			// Act
			f.Import(s.Sandbox.ProvideFile("testdata/dump.sql"));
		}
	}
}