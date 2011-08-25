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
    public class StepNetResponse
    {
        public string Error { get; set; }
        public StepData Data { get; set; }
    }

    public class StepData
    {
        public Summary Summary { get; set; }
        public List<NetContent> NetContent { get; set; }        
    }

    public class Summary
    {
        public int Count { get; set; }
        public int TotalSize { get; set; }
        public int LoadTime { get; set; }
    }

    public class NetContent
    {
        public Started Started { get; set; }
        public Stats Resolving { get; set; }
        public Stats Connecting { get; set; }
        public Stats Blocking { get; set; }
        public Stats Sending { get; set; }
        public Stats Waiting { get; set; }
        public Receiving Receiving { get; set; }
        public Started ContentLoad {get; set;}
        public Started WindowLoad { get; set; }

        public int Size { get; set; }
        public int Duration { get; set; }
        public string Domain { get; set; }
        public string Href { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
    }

    public class Started
    {
        public int Start { get; set; }
    }

    public class Stats
    {
        public int Elapsed { get; set; }
        public int Start { get; set; }
    }

    public class Receiving
    {
        public int Elapsed { get; set; }
        public int Start { get; set; }
        public bool Loaded { get; set; }
        public bool FromCache { get; set; }
    }
}
