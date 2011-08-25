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
using MonitisAPIWrapper.CustomMonitors;
using MonitisAPIWrapper.CustomMonitors.Parameters;
using MonitisAPIWrapper.CustomMonitors.Post;
using MonitisAPIWrapper.Test.DependencyInjection;
using NUnit.Framework;

namespace MonitisAPIWrapper.Test.CustomMonitorsTests
{
    [TestFixture]
    public class MonitorDeleterTest : InjectedTest
    {

        [Test]
        public void CanDeleteMonitor()
        {
            // set up
            var adder = Container.Resolve<CustomMonitorAdder>();
            adder.Name = "simple_custom_monitor";
            adder.Tag = "website position checkers";

            adder.MonitorParams.Add(new CustomMonitorParam
                                        {
                                            Name = "search_engine",
                                            DisplayName = "Search Engine",
                                            Value = "www.google.com",
                                            DataType = DataType.String,
                                            IsHidden = false
                                        });
            adder.ResultParams.Add(new ResultParam
                                       {
                                           Name = "position",
                                           DisplayName = "Position",
                                           DataType = DataType.String,
                                           UnitOfMeasure = "N/A"
                                       });
            adder.Add();

            Assert.IsTrue(adder.Success);
            Assert.IsTrue(adder.MonitorId.HasValue);

            // delete test
            var deleter = Container.Resolve<CustomMonitorDeleter>();
            deleter.MonitorId = adder.MonitorId.Value;
            deleter.RequestSender.Validation = Validation.token;

            Assert.DoesNotThrow(deleter.Delete);
            Assert.IsTrue(deleter.Success);
        }      
    }
}
