using System;
using System.IO;
using System.Runtime.Remoting;

namespace F2F.Testing.AppDomainIsolation
{
	/// <summary>Isolates a test fixture by loading it in its own app domain</summary>
	public class AppDomainIsolator<T>
		where T : MarshalByRefObject
	{
		private AppDomain mAppDomain;

		// ------------------------------------------------------------------------------

		/// <summary>Loads an instance of T into its own app domain.</summary>
		/// <returns>.</returns>
		public T Load()
		{
			string assemblyFile = typeof(T).Assembly.Location;

			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = Path.GetDirectoryName(assemblyFile);

			this.mAppDomain = AppDomain.CreateDomain("MyDomain", null, setup);

			ObjectHandle oh = this.mAppDomain.CreateInstanceFrom(assemblyFile, typeof(T).FullName);
			object obj = oh.Unwrap();

			return (T)obj;
		}

		// ------------------------------------------------------------------------------

		/// <summary>Unloads the app domain.</summary>
		public void Unload()
		{
			AppDomain.Unload(this.mAppDomain);
			this.mAppDomain = null;
		}
	}
}