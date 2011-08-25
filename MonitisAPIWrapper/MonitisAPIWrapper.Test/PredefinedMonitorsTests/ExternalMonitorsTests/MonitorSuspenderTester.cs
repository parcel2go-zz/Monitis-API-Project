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
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Post;
using MonitisAPIWrapper.Test.DependencyInjection;
using NUnit.Framework;
using Autofac;

namespace MonitisAPIWrapper.Test.PredefinedMonitorsTests.ExternalMonitorsTests
{
    [TestFixture]
    public class MonitorSuspenderTester : InjectedTest
        // ExternalMonitorDependentTest
    {
        [Test]
        public void CanSuspendMonitorUsingOneId()
        {
            var suspender = Container.Resolve<ExternalMonitorSuspender>();
            suspender.MonitorIds.Add(39333);
           
            Assert.DoesNotThrow(suspender.Suspend);
            Assert.IsTrue(suspender.Success);
        }

        [Test]
        public void CanSuspendMonitorUsingMultipleIds()
        {
            var suspender = Container.Resolve<ExternalMonitorSuspender>();
            suspender.MonitorIds.Add(39333);
            suspender.MonitorIds.Add(49378);
            suspender.MonitorIds.Add(58304);

            Assert.DoesNotThrow(suspender.Suspend);
            Assert.IsTrue(suspender.Success);
        }

        [Test]
        public void CanSuspendMonitorUsingTag()
        {
            var suspender = Container.Resolve<ExternalMonitorSuspender>();
            suspender.Tag = "default";

            Assert.DoesNotThrow(suspender.Suspend);
            Assert.IsTrue(suspender.Success);
        }
    }
}
