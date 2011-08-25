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


namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Responses
{
    public class ExternalMonitorResultsResponse
    {
        public int Id { get; set; }
        public Trend Trend { get; set; }
        public object[] Data { get; set; }
        public string LocationName { get; set; }
        public object[] AddDatas { get; set; }
    }

    public class Trend
    {
        public double Min { get; set; }
        public int OkCount { get; set; }
        public double Max { get; set; }
        public double OkSum { get; set; }
        public double NokCount { get; set; }
    }     
}
