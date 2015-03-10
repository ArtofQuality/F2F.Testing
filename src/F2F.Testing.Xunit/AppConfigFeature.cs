﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if NUNIT
using NUnit.Framework;
namespace F2F.Testing.NUnit
#endif

#if XUNIT
namespace F2F.Testing.Xunit
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace F2F.Testing.MSTest
#endif

{
	/// <summary>
	/// Replaces the app.config of current test assembly with a new file.
	/// </summary>
	public class AppConfigFeature : IDisposable
	{
		private readonly string _appConfigFile;

		private readonly string _appConfigBackupFile;

		private bool _disposed = false;

		/// <summary>
		/// Initializes the test fixture by creating a backup of app.config file.
		/// </summary>
		public AppConfigFeature()
		{
			_appConfigFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			_appConfigBackupFile = String.Format("{0}.bak", _appConfigFile);

#if XUNIT
			SetUpAppConfig();
#endif
		}

#if NUNIT

		/// <summary>Set up the feature.</summary>
		[SetUp]
		public void NUnit_SetUpAppConfig()
		{
			SetUpAppConfig();
		}

		/// <summary>Tear down the feature.</summary>
		[TearDown]
		public void NUnit_TearDownAppConfig()
		{
			Dispose();
		}

#endif

#if MSTEST

		/// <summary>Set up the feature.</summary>
		[TestInitialize]
		public void MSTest_SetUpAppConfig()
		{
			SetUpAppConfig();
		}

		/// <summary>Tear down the feature.</summary>
		[TestCleanup]
		public void MSTest_TearDownAppConfig()
		{
			Dispose();
		}

#endif

		private void SetUpAppConfig()
		{
			if (File.Exists(_appConfigFile))
			{
				File.Copy(_appConfigFile, _appConfigBackupFile, true);
			}
		}

		private void TearDownAppConfig()
		{
			if (File.Exists(_appConfigFile))
			{
				File.Delete(_appConfigFile);
			}

			if (File.Exists(_appConfigBackupFile))
			{
				File.Move(_appConfigBackupFile, _appConfigFile);
			}
		}

		/// <summary>
		/// Install new app.config for current test assembly.
		/// </summary>
		/// <param name="appConfigFile">The new app.config file.</param>
		public void Install(string appConfigFile)
		{
			File.Copy(appConfigFile, _appConfigFile, true);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		/// <seealso cref="M:System.IDisposable.Dispose()"/>
		public void Dispose()
		{
			if (!_disposed)
			{
				TearDownAppConfig();

				_disposed = true;
			}
		}
	}
}