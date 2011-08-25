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
using NUnit.Framework;
using Autofac;

namespace MonitisAPIWrapper.Test.DependencyInjection
{
    [TestFixture]
    public class ExternalMonitorDependentTest : InjectedTest
    {
        protected int MonitorId;
        protected string MonitorTag;

        [TestFixtureSetUp]
        protected override void FixtureSetUp()
        {
            base.FixtureSetUp();

            var adder = Container.Resolve<ExternalMonitorAdder>();
            adder.Type = RequestType.http;
            adder.Name = "test";
            adder.Url = "bbc.co.uk";
            adder.Interval = Interval.Five;
            adder.LocationIds.Add(52);
            adder.Tag = "hello";
            adder.RequestSender.Validation = Validation.HMACSHA1;
            adder.Add();

            if (adder.TestId != null)
            {
                MonitorTag = adder.Tag;
                MonitorId = adder.TestId.Value;
            }
        }

        [TestFixtureTearDown]
        protected override void FixtureTearDown()
        {
            var deleter = Container.Resolve<ExternalMonitorDeleter>();
            deleter.TestIds.Add(MonitorId);
            deleter.Delete();
            
            base.FixtureTearDown();
        }
    }
}
