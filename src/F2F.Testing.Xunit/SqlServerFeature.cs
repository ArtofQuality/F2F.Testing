using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
	/// Provides a temporary database in SQL Server for a test fixture.
	/// </summary>
	public class SqlServerFeature : IDisposable
	{
		private readonly string _connectionString;

		private readonly string _databaseName;

		private bool _disposed = false;

		/// <summary>
		/// Initializes the test fixture with the given SQL Server connection string.
		/// </summary>
		/// <param name="connectionString">The SQL Server connection string.</param>
		public SqlServerFeature(string connectionString)
			: this(connectionString, Guid.NewGuid().ToString())
		{
		}

		/// <summary>
		/// Initializes the test fixture with the given SQL Server connection string and database name.
		/// </summary>
		/// <param name="connectionString">The SQL Server connection string.</param>
		/// <param name="databaseName">The database name to create.</param>
		public SqlServerFeature(string connectionString, string databaseName)
		{
			_connectionString = connectionString;
			_databaseName = databaseName;

#if XUNIT
			SetUpSQLServer();
#endif
		}

		/// <summary>Gets the database.</summary>
		/// <value>The database.</value>
		public string Database
		{
			get { return _databaseName; }
		}

#if NUNIT

		/// <summary>Set up the SQL connection.</summary>
		[SetUp]
		public void NUnit_SetUpSQLServer()
		{
			SetUpSQLServer();
		}

		/// <summary>Tear down the SQL connection.</summary>
		[TearDown]
		public void NUnit_TearDownSQLServer()
		{
			Dispose();
		}

#endif

#if MSTEST

		/// <summary>Set up the SQL connection.</summary>
		[TestInitialize]
		public void MSTest_SetUpSQLServer()
		{
			SetUpSQLServer();
		}

		/// <summary>Tear down the SQL connection.</summary>
		[TestCleanup]
		public void MSTest_TearDownSQLServer()
		{
			Dispose();
		}

#endif

		private void SetUpSQLServer()
		{
			string query = String.Format("CREATE DATABASE [{0}]", _databaseName);

			using (SqlConnection con = new SqlConnection(_connectionString))
			using (SqlCommand cmd = new SqlCommand(query, con))
			{
				con.Open();

				cmd.ExecuteNonQuery();

				con.Close();
			}
		}

		private void TearDownSQLServer()
		{
			string query1 = String.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", _databaseName);
			string query2 = String.Format("DROP DATABASE [{0}]", _databaseName);

			using (SqlConnection con = new SqlConnection(_connectionString))
			using (SqlCommand cmd1 = new SqlCommand(query1, con))
			using (SqlCommand cmd2 = new SqlCommand(query2, con))
			{
				con.Open();

				cmd1.ExecuteNonQuery();
				cmd2.ExecuteNonQuery();

				con.Close();
			}
		}

		/// <summary>
		/// Import a SQL dump file into database.
		/// </summary>
		/// <param name="sqlDumpFile">The SQL dump file.</param>
		public void Import(string sqlDumpFile)
		{
			string tmpConnectionString = String.Format("{0};Database={1}", _connectionString, _databaseName);

			using (SqlConnection con = new SqlConnection(tmpConnectionString))
			using (StreamReader file = new StreamReader(sqlDumpFile))
			{
				con.Open();

				string line = file.ReadLine();
				while (line != null)
				{
					if (!String.IsNullOrEmpty(line))
					{
						using (SqlCommand cmd = new SqlCommand(line, con))
						{
							cmd.ExecuteNonQuery();
						}
					}

					line = file.ReadLine();
				}

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
				TearDownSQLServer();

				_disposed = true;
			}
		}
	}
}