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

namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Responses
{
    public class SnapshotResponse
    {
        public Datum[] Data { get; set; }
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string LocationShortName { get; set; }
    }

    public class Datum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Perf { get; set; }
        public string Status { get; set; }
        public string Tag { get; set; }
        public string Time { get; set; }
        public int Timeout { get; set; }
    }
}
