using System;
using System.Collections.Generic;
using System.Text;

#if NUNIT
using NUnit.Framework;
using System.Reflection;
namespace F2F.Testing.NUnit
#endif

#if XUNIT
namespace F2F.Testing.Xunit
#endif

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
namespace F2F.Testing.MSTest
#endif

{
	/// <summary>
	/// A generic base class for a nunit test fixture.
	/// </summary>
	public abstract class TestFixture : IDisposable
	{
		private readonly IList<object> _features = new List<object>();

		private readonly IDictionary<string, object> _namedFeatures = new Dictionary<string, object>();

		private bool _disposed = false;

		/// <summary>
		/// Register the given feature.
		/// </summary>
		/// <typeparam name="TFeature">The feature's type.</typeparam>
		/// <param name="feature">The feature.</param>
		public void Register<TFeature>(TFeature feature)
			where TFeature : class
		{
			_features.Add(feature);
		}

		/// <summary>
		/// Register the given feature by name.
		/// </summary>
		/// <typeparam name="TFeature">The feature's type.</typeparam>
		/// <param name="feature">The feature.</param>
		/// <param name="name">The feature's name.</param>
		public void Register<TFeature>(TFeature feature, string name)
			where TFeature : class
		{
			_namedFeatures.Add(name, feature);
		}

		/// <summary>
		/// Return a known feature.
		/// </summary>
		/// <typeparam name="TFeature">The feature's type.</typeparam>
		/// <returns>The feature</returns>
		public TFeature Use<TFeature>()
			where TFeature : class
		{
			foreach (object f in _features)
			{
				if (f is TFeature)
				{
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
			where TFeature : class
		{
			return _namedFeatures.ContainsKey(name)
				? (TFeature)_namedFeatures[name]
				: default(TFeature);
		}

#if NUNIT

		/// <summary>Set up all known features.</summary>
		[SetUp]
		public void SetUpFeatures()
		{
			InvokeMethodWithAttribute<SetUpAttribute>(_features);
			InvokeMethodWithAttribute<SetUpAttribute>(_namedFeatures.Values);
		}

		/// <summary>Tear down all known features.</summary>
		[TearDown]
		public void TearDownFeatures()
		{
			InvokeMethodWithAttribute<TearDownAttribute>(_features);
			InvokeMethodWithAttribute<TearDownAttribute>(_namedFeatures.Values);
		}

#endif

#if MSTEST

		/// <summary>Set up all known features.</summary>
		[TestInitialize]
		public void SetUpFeatures()
		{
			InvokeMethodWithAttribute<TestInitializeAttribute>(_features);
			InvokeMethodWithAttribute<TestInitializeAttribute>(_namedFeatures.Values);
		}

		/// <summary>Tear down all known features.</summary>
		[TestCleanup]
		public void TearDownFeatures()
		{
			InvokeMethodWithAttribute<TestCleanupAttribute>(_features);
			InvokeMethodWithAttribute<TestCleanupAttribute>(_namedFeatures.Values);
		}

#endif
		
#if !XUNIT

		/// <summary>Invoke all methods with support given attribute.</summary>
		private static void InvokeMethodWithAttribute<TAttribute>(IEnumerable<object> features)
		{
			foreach (object f in features)
			{
				foreach (MethodInfo m in f.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
				{
					foreach (object a in m.GetCustomAttributes(true))
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
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		/// <seealso cref="M:System.IDisposable.Dispose()"/>
		public void Dispose()
		{
			if (!_disposed)
			{
				Dispose(_features);
				Dispose(_namedFeatures.Values);

				_disposed = true;
			}
		}

		/// <summary>Dispose all features which are IDisposable.</summary>
		private static void Dispose(IEnumerable<object> features)
		{
			foreach (object f in features)
			{
				if (f is IDisposable)
				{
					((IDisposable)f).Dispose();
				}
			}
		}
	}
}