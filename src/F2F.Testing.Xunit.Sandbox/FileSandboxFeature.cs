using System;
using System.Collections.Generic;
using System.Text;
using F2F.Sandbox;

#if NUNIT

using NUnit.Framework;

namespace F2F.Testing.NUnit.Sandbox
#endif

#if XUNIT
namespace F2F.Testing.Xunit.Sandbox
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace F2F.Testing.MSTest.Sandbox
#endif

{
	/// <summary>
	/// Provides a temporary file system (sandbox) for a test fixture.
	/// </summary>
	public class FileSandboxFeature : IDisposable
	{
		/// <summary>The file locator.</summary>
		private readonly IFileLocator _fileLocator;

		/// <summary>The disposed state.</summary>
		private bool _disposed = false;

		/// <summary>The sandbox.</summary>
		private IFileSandbox _sandbox;

		/// <summary>
		/// Initializes the test fixture without a file locator.
		/// </summary>
		public FileSandboxFeature()
			: this(new EmptyFileLocator())
		{
		}

		/// <summary>
		/// Initializes the test fixture with the given file locator.
		/// </summary>
		/// <param name="fileLocator">The file locator.</param>
		public FileSandboxFeature(IFileLocator fileLocator)
		{
			_fileLocator = fileLocator;

#if XUNIT
			SetUpSandbox();
#endif
		}

		/// <summary>Gets the sandbox.</summary>
		/// <value>The sandbox.</value>
		public IFileSandbox Sandbox
		{
			get { return _sandbox; }
		}

#if NUNIT

		/// <summary>Set up the sandbox.</summary>
		[SetUp]
		public void NUnit_SetUpSandbox()
		{
			SetUpSandbox();
		}

		/// <summary>Tear down the sandbox.</summary>
		[TearDown]
		public void NUnit_TearDownSandbox()
		{
			Dispose();
		}

#endif

#if MSTEST

		/// <summary>Set up the sandbox.</summary>
		[TestInitialize]
		public void MSTest_SetUpSandbox()
		{
			SetUpSandbox();
		}

		/// <summary>Tear down the sandbox.</summary>
		[TestCleanup]
		public void MSTest_TearDownSandbox()
		{
			Dispose();
		}

#endif

		private void SetUpSandbox()
		{
			_sandbox = new FileSandbox(_fileLocator);
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
				_sandbox.Dispose();

				_disposed = true;
			}
		}
	}
}