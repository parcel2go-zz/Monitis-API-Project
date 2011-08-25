///    copyright parcel2go.com 2011 www.parcel2go.com
///
///    This program is free software: you can redistribute it and/or modify
///    it under the terms of the GNU General Public License as published by
///    the Free Software Foundation, either version 3 of the License, or
///    (at your option) any later version.

///    This program is distributed in the hope that it will be useful,
///    but WITHOUT ANY WARRANTY; without even the implied warranty of
///    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
///    GNU General Public License for more details.

///    You should have received a copy of the GNU General Public License
///    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using MonitisAPIWrapper.CustomMonitors;
using MonitisAPIWrapper.CustomMonitors.Parameters;
using MonitisAPIWrapper.CustomMonitors.Post;
using NUnit.Framework;
using Autofac;

namespace MonitisAPIWrapper.Test.DependencyInjection
{
    [TestFixture]
    public class CustomMonitorDependentTest : InjectedTest
    {
        protected int MonitorId;

        [TestFixtureSetUp]
        protected override void FixtureSetUp()
        {
            base.FixtureSetUp();

            var adder = Container.Resolve<CustomMonitorAdder>();
            adder.Name = "simple_custom_monitor";
            adder.Tag = "website position checkers";
            adder.MonitorParams.Add(new CustomMonitorParam { Name = "search_engine", DisplayName = "Search Engine", Value = "www.google.com", DataType = DataType.String, IsHidden = false });
            adder.ResultParams.Add(new ResultParam { Name = "position", DisplayName = "Position", DataType = DataType.String, UnitOfMeasure = "N/A" });
            adder.Add();

            if (adder.MonitorId != null) MonitorId = adder.MonitorId.Value;
        }

        [TestFixtureTearDown]
        protected override void FixtureTearDown()
        {
            var deleter = Container.Resolve<CustomMonitorDeleter>();
            deleter.MonitorId = MonitorId;
            deleter.Delete();

            base.FixtureTearDown();
        }

    }
}
