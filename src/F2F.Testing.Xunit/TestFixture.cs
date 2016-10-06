using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;

#if NUNIT
using NUnit.Framework;
using System.Reflection;
namespace F2F.Testing.NUnit
#endif

#if XUNIT || XUNIT2
namespace F2F.Testing.Xunit
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
namespace F2F.Testing.MSTest
#endif
{
	/// <summary>
	/// A generic base class for a test fixture.
	/// </summary>
	public abstract class TestFixture : IDisposable
	{
		private class NamedFeature
		{
			public string Name { get; set; }
			public IFeature Feature { get; set; }
		}

		private LinkedList<IFeature> _features = new LinkedList<IFeature>();
		private LinkedList<NamedFeature> _namedFeatures = new LinkedList<NamedFeature>();

		private bool _disposed = false;

		/// <summary>
		/// Register the given feature.
		/// </summary>
		/// <typeparam name="TFeature">The feature's type.</typeparam>
		/// <param name="feature">The feature.</param>
		public void Register<TFeature>(TFeature feature)
			where TFeature : class, IFeature
        {
			if (_disposed) throw new ObjectDisposedException("TestFixture");

			_features.AddLast(feature);
		}

		/// <summary>
		/// Register the given feature by name.
		/// </summary>
		/// <typeparam name="TFeature">The feature's type.</typeparam>
		/// <param name="feature">The feature.</param>
		/// <param name="name">The feature's name.</param>
		public void Register<TFeature>(TFeature feature, string name)
			where TFeature : class, IFeature
        {
			if (_disposed) throw new ObjectDisposedException("TestFixture");

			_namedFeatures.AddLast(new NamedFeature
			{
				Name = name,
				Feature = feature
			});
		}

		/// <summary>
		/// Return a known feature.
		/// </summary>
		/// <typeparam name="TFeature">The feature's type.</typeparam>
		/// <returns>The feature</returns>
		public TFeature Use<TFeature>()
			where TFeature : class, IFeature
        {
			if (_disposed) throw new ObjectDisposedException("TestFixture");

			foreach (var f in _features)
			{
				if (f is TFeature)
				{
                    f.OnUse();
					return f as TFeature;
				}
			}

			return default(TFeature);
		}

		/// <summary>
		/// Return a known feature.
		/// </summary>
		/// <typeparam name="TFeature">The feature's type.</typeparam>
		/// <param name="name">The feature's name.</param>
		/// <returns>The feature</returns>
		public TFeature Use<TFeature>(string name)
			where TFeature : class, IFeature
        {
			if (_disposed) throw new ObjectDisposedException("TestFixture");

			// TODO: Probably inefficient, but not critical, since it should be only a few named features registered
			foreach (var feature in _namedFeatures)
			{
				if (String.CompareOrdinal(feature.Name, name) == 0 && feature.Feature is TFeature)
				{
                    feature.Feature.OnUse();
                    return feature as TFeature;
				}
			}

			return default(TFeature);
		}

#if NUNIT

		/// <summary>Set up all known features.</summary>
		[SetUp]
		public void SetUpFeatures()
		{
			InvokeMethodWithAttribute<SetUpAttribute>(_features);
			InvokeMethodWithAttribute<SetUpAttribute>(_namedFeatures.Select(x => x.Feature));
		}

		/// <summary>Tear down all known features.</summary>
		[TearDown]
		public void TearDownFeatures()
		{
			InvokeMethodWithAttribute<TearDownAttribute>(_features);
			InvokeMethodWithAttribute<TearDownAttribute>(_namedFeatures.Select(x => x.Feature));
		}

		/// <summary>Dispose all known features.</summary>
		[TestFixtureTearDown]
		public void DisposeFeatures()
		{
			Dispose();
		}

#endif

#if MSTEST

		/// <summary>Set up all known features.</summary>
		[TestInitialize]
		public void SetUpFeatures()
		{
			InvokeMethodWithAttribute<TestInitializeAttribute>(_features);
			InvokeMethodWithAttribute<TestInitializeAttribute>(_namedFeatures.Select(x => x.Feature));
		}

		/// <summary>Tear down all known features.</summary>
		[TestCleanup]
		public void TearDownFeatures()
		{
			InvokeMethodWithAttribute<TestCleanupAttribute>(_features);
			InvokeMethodWithAttribute<TestCleanupAttribute>(_namedFeatures.Select(x => x.Feature));

			Dispose();
		}

#endif

#if !XUNIT && !XUNIT2

		/// <summary>Invoke all methods with support given attribute.</summary>
		private static void InvokeMethodWithAttribute<TAttribute>(IEnumerable<IFeature> features)
		{
			foreach (var f in features)
			{
				foreach (MethodInfo m in f.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
				{
					foreach (var a in m.GetCustomAttributes(true))
					{
						if (a is TAttribute)
						{
							m.Invoke(f, new object[0]);
						}
					}
				}
			}
		}

#endif
		/// <summary>
		/// Class destructor. Calls Dispose, no managed resources should be disposed
		/// </summary>
		~TestFixture()
		{
			Dispose(false);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		/// <seealso cref="M:System.IDisposable.Dispose()"/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				if (_features != null)
				{
					DisposeInReverseOrder(_features);
					_features = null;
				}
				if (_namedFeatures != null)
				{
					DisposeInReverseOrder(_namedFeatures.Select(x => x.Feature));
					_namedFeatures = null;
				}

				_disposed = true;
			}
		}

		/// <summary>Dispose all features which are IDisposable in reverse order.</summary>
		private static void DisposeInReverseOrder(IEnumerable<IFeature> features)
		{
			foreach (object f in features.Reverse())
			{
				if (f is IDisposable)
				{
					((IDisposable)f).Dispose();
				}
			}
		}
	}
}