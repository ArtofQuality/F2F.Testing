using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using F2F.Sandbox;

#if NUNIT
using NUnit.Framework;
namespace F2F.Testing.NUnit.EF
#endif

#if XUNIT || XUNIT2
namespace F2F.Testing.Xunit.EF
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace F2F.Testing.MSTest.EF
#endif

{
	/// <summary>
	/// Provides a temporary file for local db SQL connection.
	/// </summary>
	public class LocalDbFeature : IDisposable
	{
		private IFileSandbox _sandbox;
		private string _databaseFile;
		private string _connectionString;

		/// <summary>
		/// Initializes a file sandbox containing a temporary file.
		/// </summary>
		public LocalDbFeature()
		{
#if XUNIT || XUNIT2
			SetUpLocalDb();
#endif
		}

#if NUNIT

		/// <summary>Set up the database.</summary>
		[SetUp]
		public void NUnit_SetUpLocalDb()
		{
			SetUpLocalDb();
		}

		/// <summary>Tear down the database.</summary>
		[TearDown]
		public void NUnit_TearDownLocalDb()
		{
			Dispose();
		}

#endif

#if MSTEST

		/// <summary>Set up the database.</summary>
		[TestInitialize]
		public void MSTest_SetUpLocalDb()
		{
			SetUpLocalDb();
		}

		/// <summary>Tear down the database.</summary>
		[TestCleanup]
		public void MSTest_TearDownLocalDb()
		{
			Dispose();
		}

#endif

		private void SetUpLocalDb()
		{
			_sandbox = new FileSandbox(new EmptyFileLocator());
			_databaseFile = _sandbox.GetTempFile("mdf");
			_connectionString = String.Format(@"Data Source=(localdb)\v11.0;AttachDbFileName={0};Integrated Security=True;Connect Timeout=5", _databaseFile);
		}

		/// <summary>
		/// The path to local db file.
		/// </summary>
		public string DatabaseFile
		{
			get { return _databaseFile; }
		}

		/// <summary>
		/// The connection string to local db.
		/// </summary>
		public string ConnectionString
		{
			get { return _connectionString; }
		}

		/// <summary>
		/// Import a SQL dump file into database.
		/// </summary>
		/// <param name="sqlDumpFile">The SQL dump file.</param>
		public void Import(string sqlDumpFile)
		{
			if (String.IsNullOrEmpty(sqlDumpFile))
				throw new ArgumentException("sqlDumpFile is null or empty.", "sqlDumpFile");

			using (var con = new SqlConnection(ConnectionString))
			using (var file = new StreamReader(sqlDumpFile))
			{
				con.Open();
				var transaction = con.BeginTransaction();

				var line = file.ReadLine();
				while (line != null)
				{
					if (!String.IsNullOrEmpty(line))
					{
						using (var cmd = con.CreateCommand())
						{
							cmd.CommandText = line;
							cmd.Transaction = transaction;
							cmd.ExecuteNonQuery();
						}
					}
					line = file.ReadLine();
				}

				transaction.Commit();
				con.Close();
			}
		}

		/// <summary>
		/// Dispose file sandbox.
		/// </summary>
		public virtual void Dispose()
		{
			if (_sandbox != null)
			{
				_sandbox.Dispose();
				_sandbox = null;
				_databaseFile = null;
				_connectionString = null;
			}
		}
	}
}