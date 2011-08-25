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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using JsonCSharpClassGenerator;
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Parameters;
using Newtonsoft.Json.Linq;

namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Responses
{
    public class ExternalMonitorInfoResponse
    {
        public object AuthPassword { get; set; }
        public object AuthUsername { get; set; }
        public string DetailedType { get; set; }
        public int Interval { get; set; }

        public Locations[] Locations { get; set; }

        public object Match { get; set; }
        public object MatchText { get; set; }
        public string Name { get; set; }

        public Params Params { get; set; }
        public string PostData { get; set; }
        public Sla Sla { get; set; }

        public string StartDate { get; set; }
        public string Tag { get; set; }
        public int TestId { get; set; }
        public int Timeout { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }

    public class Locations
    {
        public int CheckInterval { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Params
    {
        public NameValueCollection Values { get; set; }
    }

    public class Sla
    {
        public NameValueCollection Values { get; set; }
    }
}
