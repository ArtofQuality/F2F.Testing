using System;
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
    /// <inheritdoc/>
    public class Feature : IFeature
    {
        /// <inheritdoc/>
        public virtual void OnUse()
        {
            ; // no initialization in base class
        }
    }
}
