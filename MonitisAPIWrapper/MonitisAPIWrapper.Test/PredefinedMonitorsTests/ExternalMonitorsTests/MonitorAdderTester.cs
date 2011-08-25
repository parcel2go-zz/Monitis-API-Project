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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Parameters;
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Post;
using MonitisAPIWrapper.Test.DependencyInjection;
using NUnit.Framework;
using Autofac;

namespace MonitisAPIWrapper.Test.PredefinedMonitorsTests.ExternalMonitorsTests
{
    [TestFixture]
    public class MonitorAdderTester : InjectedTest
    {
        [Test]
        public void CanAddMonitor()
        {
            // add monitor 
            var adder = Container.Resolve<ExternalMonitorAdder>();
            adder.Type = RequestType.http;
            adder.Name = "test";
            adder.Url = "google.co.uk";
            adder.Interval = Interval.Five;
            adder.LocationIds.Add(52);
            adder.LocationIds.Add(2);
            adder.Tag = "tag";
            adder.RequestSender.Validation = Validation.HMACSHA1;
            adder.Add();

            Assert.IsTrue(adder.Success);
            Assert.IsNotEmpty(adder.TestId.Value.ToString());

            // clean up afterwards
            if (adder.TestId.HasValue)
            {
                var deleter = Container.Resolve<ExternalMonitorDeleter>();
                deleter.TestIds.Add(adder.TestId.Value);
                deleter.Delete();
            }
        }
    }
}
