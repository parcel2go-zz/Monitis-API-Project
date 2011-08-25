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
using MonitisAPIWrapper.CustomMonitors.Get;
using MonitisAPIWrapper.Test.DependencyInjection;
using NUnit.Framework;
using Autofac;

namespace MonitisAPIWrapper.Test.CustomMonitorsTests
{
    [TestFixture]
    internal class AdditionalMonitorResultsGetterTester : CustomMonitorDependentTest
    {
        [Test]
        public void CanGetAdditionalMonitorResults()
        {
            var getter = Container.Resolve<AdditionalCustomMonitorResultsGetter>();
            getter.MonitorId = MonitorId;
            getter.CheckTime = DateTime.Now;
            getter.RequestSender.Validation = Validation.HMACSHA1;

            Assert.DoesNotThrow(getter.Get);
            Assert.IsTrue(getter.Success);
        }      
    }
}
