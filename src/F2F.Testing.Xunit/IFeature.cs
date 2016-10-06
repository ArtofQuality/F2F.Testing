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
    /// Represents a test feature that can be registered within a <see cref="TestFixture"/>
    /// </summary>
    public interface IFeature
    {
        /// <summary>
        /// Called, when this feature is actually used, as soon as a call to <see cref="TestFixture.Use{TFeature}()"/> or
        /// <see cref="TestFixture.Use{TFeature}(string)"/> is performed. A feature implementation can use this to 
        /// perform some initialization.
        /// </summary>
        void OnUse();
    }
}
