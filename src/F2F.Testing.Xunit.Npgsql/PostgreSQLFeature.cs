using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Npgsql;

#if NUNIT
using NUnit.Framework;
namespace F2F.Testing.NUnit.Npgsql
#endif

#if XUNIT || XUNIT2
namespace F2F.Testing.Xunit.Npgsql
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace F2F.Testing.MSTest.Npgsql
#endif

{
	/// <summary>
	/// Provides a temporary database in PostgreSQL server for a test fixture.
	/// </summary>
	public class PostgreSQLFeature : Feature, IDisposable
	{
		private readonly string _databaseName;

		private readonly string _serverConnectionString;

		private readonly string _databaseConnectionString;

		private bool _disposed = false;

		/// <summary>
		/// Initializes the test fixture with the given PostgreSQL connection string.
		/// </summary>
		/// <param name="connectionString">The PostgreSQL connection string.</param>
		public PostgreSQLFeature(string connectionString)
			: this(connectionString, Guid.NewGuid().ToString())
		{
		}

		/// <summary>
		/// Initializes the test fixture with the given PostgreSQL connection string and database name.
		/// </summary>
		/// <param name="connectionString">The PostgreSQL connection string.</param>
		/// <param name="databaseName">The database name to create.</param>
		public PostgreSQLFeature(string connectionString, string databaseName)
		{
			_databaseName = databaseName;
			_serverConnectionString = connectionString;
			_databaseConnectionString = String.Format("{0};Database={1}", connectionString, databaseName);

#if XUNIT
			SetUpDatabase();
#endif
		}

		/// <summary>Gets the database name.</summary>
		/// <value>The database name.</value>
		public string Database
		{
			get { return _databaseName; }
		}

		/// <summary>Gets the connection string to temporary database.</summary>
		/// <value>The connection string.</value>
		public string ConnectionString
		{
			get { return _databaseConnectionString; }
		}

#if NUNIT

		/// <summary>Set up the database.</summary>
		[SetUp]
		public void NUnit_SetUpDatabase()
		{
			SetUpDatabase();
		}

		/// <summary>Tear down the database.</summary>
		[TearDown]
		public void NUnit_TearDownDatabase()
		{
			Dispose();
		}

#endif

#if MSTEST

		/// <summary>Set up the database.</summary>
		[TestInitialize]
		public void MSTest_SetUpDatabase()
		{
			SetUpDatabase();
		}

		/// <summary>Tear down the database.</summary>
		[TestCleanup]
		public void MSTest_TearDownDatabase()
		{
			Dispose();
		}

#endif

		private void SetUpDatabase()
		{
			var query = String.Format("CREATE DATABASE [{0}]", _databaseName);

			using (var con = new NpgsqlConnection(_serverConnectionString))
			using (var cmd = new NpgsqlCommand(query, con))
			{
				con.Open();

				cmd.ExecuteNonQuery();

				con.Close();
			}
		}

		private void TearDownDatabase()
		{
			var query = String.Format("DROP DATABASE [{0}]", _databaseName);

			using (var con = new NpgsqlConnection(_serverConnectionString))
			using (var cmd = new NpgsqlCommand(query, con))
			{
				con.Open();

				cmd.ExecuteNonQuery();

				con.Close();
			}
		}

		/// <summary>
		/// Import a SQL dump file into database.
		/// </summary>
		/// <param name="sqlDumpFile">The SQL dump file.</param>
		public void Import(string sqlDumpFile)
		{
			using (var con = new NpgsqlConnection(_databaseConnectionString))
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
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		/// <seealso cref="M:System.IDisposable.Dispose()"/>
		public void Dispose()
		{
			if (!_disposed)
			{
				TearDownDatabase();

				_disposed = true;
			}
		}
	}
}