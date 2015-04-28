using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using F2F.Sandbox;

#if NUNIT
namespace F2F.Testing.NUnit.EF
#endif

#if XUNIT

namespace F2F.Testing.Xunit.EF
#endif

#if MSTEST
namespace F2F.Testing.MSTest.EF
#endif

{
	/// <summary>
	/// Provides a temporary file for local db SQL connection.
	/// </summary>
	public class LocalDbFeature : IDisposable
	{
		private IFileSandbox _sandbox;

		private readonly string _databaseFile;
		private readonly string _connectionString;

		/// <summary>
		/// Initializes a file sandbox containing a temporary file.
		/// </summary>
		public LocalDbFeature()
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
			if (string.IsNullOrEmpty(sqlDumpFile))
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
			}
		}
	}
}