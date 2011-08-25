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

using Autofac;
using MonitisAPIWrapper.CustomMonitors.Parameters;
using MonitisAPIWrapper.CustomMonitors.Post;
using MonitisAPIWrapper.Test.DependencyInjection;
using NUnit.Framework;

namespace MonitisAPIWrapper.Test.CustomMonitorsTests
{
    [TestFixture]
    class MonitorEditorTests : InjectedTest
    {
        [Test]
        public void CanEditMonitor()
        {
            // set up
            var adder = Container.Resolve<CustomMonitorAdder>();
            adder.Name = "simple_custom_monitor";
            adder.Tag = "website position checkers";

            adder.MonitorParams.Add(new CustomMonitorParam { Name = "search_engine", DisplayName = "Search Engine", Value = "www.google.com", DataType = DataType.String, IsHidden = false });
            adder.ResultParams.Add(new ResultParam { Name = "position", DisplayName = "Position", DataType = DataType.String, UnitOfMeasure = "N/A" });
            adder.Add();

            Assert.IsTrue(adder.Success);
            Assert.IsTrue(adder.MonitorId.HasValue);

            // edit test
            var editor = Container.Resolve<CustomMonitorEditer>();
            editor.MonitorId = adder.MonitorId.Value;
            editor.Name = "simple_custom_editor";
            editor.Tag = "website_position_editors";
            editor.MonitorParams.Add(new CustomMonitorParam { Name = "search", DisplayName = "Search", Value = "www.bbc.com", DataType = DataType.String, IsHidden = true });
            editor.RequestSender.Validation = Validation.HMACSHA1;
            editor.Edit();

            // clean up
            var deleter = Container.Resolve<CustomMonitorDeleter>();
            deleter.MonitorId = adder.MonitorId.Value;
            deleter.Delete();
        }      
    }
}
