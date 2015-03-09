using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace F2F.Testing.Sandbox
{
	/// <summary>File sandbox. </summary>
	public class FileSandbox : IFileSandbox
	{
		private bool _disposed = false;

		private readonly IFileLocator _fileLocator;

		private readonly string _sandboxPath;

		public FileSandbox(IFileLocator fileLocator)
		{
			_fileLocator = fileLocator;

			_sandboxPath = Path.Combine(GetSandboxBasePath(), Guid.NewGuid().ToString());

			System.IO.Directory.CreateDirectory(_sandboxPath);
		}

		~FileSandbox()
		{
			Dispose(false);
		}

		public string Directory
		{
			get { return _sandboxPath; }
		}

		public IFileLocator FileLocator
		{
			get { return _fileLocator; }
		}

		public string ResolvePath(string fileName)
		{
			return Path.Combine(_sandboxPath, fileName);
		}

		public bool ExistsFile(string fileName)
		{
			return System.IO.File.Exists(ResolvePath(fileName));
		}

		public bool ExistsDirectory(string fileName)
		{
			return System.IO.Directory.Exists(ResolvePath(fileName));
		}

		public string CreateFile(string fileName)
		{
			string sandboxFile = ResolvePath(fileName);

			CreateFileInSandbox(sandboxFile);

			return sandboxFile;
		}

		public string CreateTempFile()
		{
			string sandboxFile = GetTempFile();

			CreateFileInSandbox(sandboxFile);

			return sandboxFile;
		}

		public string CreateTempFile(string fileExtension)
		{
			string sandboxFile = GetTempFile(fileExtension);

			CreateFileInSandbox(sandboxFile);

			return sandboxFile;
		}

		public string CreateDirectory(string directoryName)
		{
			string createdDirectory = ResolvePath(directoryName);

			CreateDirectoryIfNotExists(createdDirectory);

			return createdDirectory;
		}

		public IEnumerable<string> CreateDirectories(params string[] directories)
		{
			IList<string> createdDirectories = new List<string>();
			foreach (string directory in directories)
			{
				createdDirectories.Add(ProvideDirectory(directory));
			}
			return createdDirectories;
		}

		public string GetTempFile()
		{
			return Path.Combine(_sandboxPath, Guid.NewGuid().ToString());
		}

		public string GetTempFile(string fileExtension)
		{
			return Path.ChangeExtension(GetTempFileName(), fileExtension);
		}

		public string GetTempFileName()
		{
			return GetTempFile();
		}

		public string GetTempFileName(string fileExtension)
		{
			return GetTempFile(fileExtension);
		}

		public string ProvideFile(string fileName)
		{
			if (!_fileLocator.Exists(fileName))
			{
				throw new FileNotFoundException(String.Format("file {0} not found in sandbox", fileName));
			}

			string sandboxFile = ResolvePath(fileName);

			CreateDirectoryIfNotExists(Path.GetDirectoryName(sandboxFile));

			if (sandboxFile != fileName)
			{
				_fileLocator.CopyTo(fileName, sandboxFile);
			}

			return sandboxFile;
		}

		public IEnumerable<string> ProvideFiles(params string[] fileNames)
		{
			IList<string> resolvedFiles = new List<string>();
			foreach (string fileName in fileNames)
			{
				resolvedFiles.Add(ProvideFile(fileName));
			}
			return resolvedFiles;
		}

		public string ProvideDirectory(string directoryName)
		{
			var sandboxDirectory = ResolvePath(directoryName);

			CreateDirectoryIfNotExists(sandboxDirectory);

			foreach (var file in _fileLocator.EnumeratePath(directoryName))
			{
				ProvideFile(file);
			}

			return sandboxDirectory;
		}

		public IEnumerable<string> ProvideDirectories(params string[] directoryNames)
		{
			IList<string> resolvedDirectories = new List<string>();
			foreach (string directory in directoryNames)
			{
				resolvedDirectories.Add(ProvideDirectory(directory));
			}
			return resolvedDirectories;
		}

		private static string GetSandboxBasePath()
		{
			return Path.GetTempPath();
		}

		private void CreateDirectoryIfNotExists(string sandboxDirectory)
		{
			if (!System.IO.Directory.Exists(sandboxDirectory))
			{
				System.IO.Directory.CreateDirectory(sandboxDirectory);
			}
		}

		private void CreateFileInSandbox(string sandboxFile)
		{
			CreateDirectoryIfNotExists(Path.GetDirectoryName(sandboxFile));

			using (System.IO.File.Create(sandboxFile))
			{
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				ReleaseUnmanagedResources();

				_disposed = true;
			}
		}

		private void ReleaseUnmanagedResources()
		{
			var di = new DirectoryInfo(_sandboxPath);
			if (di.Exists)
			{
				try
				{
					di.Delete(true);
				}
				catch
				{
					// ignore
				}
			}
		}
	}
}