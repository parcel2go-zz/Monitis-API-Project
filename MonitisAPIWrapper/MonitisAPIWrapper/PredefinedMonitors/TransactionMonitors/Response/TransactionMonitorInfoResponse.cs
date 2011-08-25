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

namespace MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Response
{
    public class TransactionMonitorInfoResponse
    {
        public string StartDate { get; set; }
        public int TestId { get; set; }
        public Locations[] Locations { get; set; }
        public string Tag { get; set; }

        public string Name { get; set; }
        public int Sla { get; set; }
        public string Url { get; set; }
    }

    public class Locations
    {
        public int Id { get; set; }
        public int CheckInterval { get; set; }
        public string Name {get; set;}
        public string FullName { get; set; }
    }
}
