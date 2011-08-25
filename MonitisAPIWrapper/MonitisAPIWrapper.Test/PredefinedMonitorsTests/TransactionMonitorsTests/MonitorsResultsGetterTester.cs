﻿///    copyright parcel2go.com 2011 www.parcel2go.com
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
using NUnit.Framework;
using Autofac;
using MonitisAPIWrapper.Test.DependencyInjection;
using MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Get;

namespace MonitisAPIWrapper.Test.PredefinedMonitorsTests.TransactionMonitorsTests
{
    [TestFixture]
    class MonitorsResultsGetterTester : InjectedTest
    {
        [Test]
        public void CanGetMonitorResults()
        {
            var getter = Container.Resolve<TransactionMonitorResultsGetter>();
            getter.MonitorId = 1;          
            getter.Date = DateTime.Now;

            Assert.DoesNotThrow(getter.Get);
            /*
            Assert.IsTrue(getter.Successful);
            */
        }
    }
}
