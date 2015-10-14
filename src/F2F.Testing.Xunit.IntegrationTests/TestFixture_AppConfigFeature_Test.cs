﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using F2F.Sandbox;
using FluentAssertions;

#if NUNIT
using NUnit.Framework;
using F2F.Testing.NUnit.Sandbox;
namespace F2F.Testing.NUnit.IntegrationTests
#endif

#if XUNIT || XUNIT2
using Xunit;
using F2F.Testing.Xunit.Sandbox;
namespace F2F.Testing.Xunit.IntegrationTests
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using F2F.Testing.MSTest.Sandbox;
namespace F2F.Testing.MSTest.IntegrationTests
#endif

{
#if NUNIT
	[TestFixture]
#endif
#if MSTEST
	[TestClass]
#endif
	public class TestFixture_AppConfigFeature_Test : TestFixture
	{
		public TestFixture_AppConfigFeature_Test()
		{
			Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
			Register(new AppConfigFeature());
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
		public void WhenInstallingAppConfig_ShouldContainCorrectData()
		{
			// Arrange
			var appConfig = Use<FileSandboxFeature>().Sandbox.ProvideFile("testdata/app1.config");
			var sut = Use<AppConfigFeature>();

			ConfigurationManager.AppSettings.Should().BeEmpty();

			// Act
			sut.Install(appConfig);

			// Assert
			ConfigurationManager.AppSettings["count"].Should().Be("7");
		}
	}
}