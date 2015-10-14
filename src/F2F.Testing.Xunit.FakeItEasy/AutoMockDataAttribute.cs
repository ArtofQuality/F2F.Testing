using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
#if XUNIT
using Ploeh.AutoFixture.Xunit;
#endif
#if XUNIT2
using Ploeh.AutoFixture.Xunit2;
#endif

namespace F2F.Testing.Xunit.FakeItEasy
{
	/// <summary>
	/// Class AutoMockDataAttribute.
	/// </summary>
	public class AutoMockDataAttribute : AutoDataAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AutoMockDataAttribute"/> class.
		/// </summary>
		/// <remarks>This constructor overload initializes the <see cref="P:Ploeh.AutoFixture.Xunit.AutoDataAttribute.Fixture" /> to an instance of
		/// <see cref="P:Ploeh.AutoFixture.Xunit.AutoDataAttribute.Fixture" />.</remarks>
		public AutoMockDataAttribute()
			: base(new Fixture()
				.Customize(new AutoFakeItEasyCustomization()))
		{
		}
	}
}