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

namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Parameters
{
    /// <summary>
    /// Only used for dns and mysql requests
    /// </summary>
    public class Params
    {
        public string Username { get; set; } // username for authentication
        public string Password { get; set; } // password for authentication
        public int? Port { get; set; } // mysql port number
        public int? TimeOut { get; set; } // mysql timeout in seconds for dns test
        public string Server { get; set; } // the name server
        public string ExpectedIP { get; set; } // the IP address expected
        public string ExPauth { get; set; } //  if name server is authoritative should be "-A" otherwise an empty string
    }
}
