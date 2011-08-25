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

using System.Collections.Generic;
using System.Collections.Specialized;
using MonitisAPIWrapper.CustomMonitors;
using MonitisAPIWrapper.CustomMonitors.Parameters;
using MonitisAPIWrapper.CustomMonitors.Post;
using MonitisAPIWrapper.Test.DependencyInjection;
using NUnit.Framework;
using Autofac;

namespace MonitisAPIWrapper.Test.CustomMonitorsTests
{
    [TestFixture]
    public class MonitorResultsAdderTests : CustomMonitorDependentTest
    {
        [Test]
        public void CanAddMonitorResults()
        {
            var resultsAdder = Container.Resolve<CustomMonitorResultsAdder>();
            resultsAdder.MonitorId = MonitorId;
            resultsAdder.RequestSender.Validation = Validation.HMACSHA1;
            
            resultsAdder.Results = new List<Results> { new Results{ParamName = "position", ParamValue = "third" }, 
                new Results{ ParamName = "position", ParamValue = "fourth" }};

            Assert.DoesNotThrow(resultsAdder.Add);
            Assert.IsTrue(resultsAdder.Success);
        }      
    }
}
