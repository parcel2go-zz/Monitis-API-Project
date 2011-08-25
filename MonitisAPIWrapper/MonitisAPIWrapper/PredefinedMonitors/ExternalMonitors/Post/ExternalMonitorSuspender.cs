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
using Common.Logging;

namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Post
{
    public class ExternalMonitorSuspender
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // sends and recieves
        public ILog Log { get; set; } // log errors

        // parameters sent
        public List<int> MonitorIds { get; set; } // only use if no value is provided for a tag, Ids of monitors to suspend
        public string Tag { get; set; } // tests with this matching tag will be suspended
        // parameters sent

        public string ResponseData { get; private set; } // response returned after request
        public bool Success { get; private set; }

        public ExternalMonitorSuspender(IRequestSender<SimpleResponse> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
            MonitorIds = new List<int>();
        }

        public void Suspend()
        {
            try
            {
                RequestSender.PostData = BuildData();
                RequestSender.Send();
                ParseResponse(RequestSender.ResponseData);
                Success = true;
            }

            catch (Exception exception)
            {
                Log.Error("Could not suspend monitor", exception);
                Success = false; 
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();

            if (! string.IsNullOrWhiteSpace(Tag)) 
                data["tag"] = Tag;

            else
                data["monitorIds"] = CreateMonitorIdsString();

            return data;
        }

        private object CreateMonitorIdsString()
        {
            var builder = new StringBuilder();

            foreach (var monitorId in MonitorIds)
            {
                builder.Append(monitorId + ",");
            }

            return builder.ToString();
        }

        private void ParseResponse(SimpleResponse response)
        {
            Success = (response.Status == "ok");

            if (!string.IsNullOrWhiteSpace(response.Data))
                ResponseData = response.Data;
        }
    }
}
