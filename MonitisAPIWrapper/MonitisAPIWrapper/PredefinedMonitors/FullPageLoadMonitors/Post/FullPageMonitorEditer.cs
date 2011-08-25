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

namespace MonitisAPIWrapper.PredefinedMonitors.FullPageLoadMonitors.Post
{
    public class FullPageMonitorEditer
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // send and recieve
        public ILog Log { get; set; } // log errors

        // parameters sent
        public int MonitorId { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public List<int> LocationIds { get; set; }
        public int CheckInterval { get; set; }
        public string Url { get; set; }
        public int Timeout { get; set; }

        public int? UptimeSla { get; set; }
        public int? ResponseSla { get; set; }
        
        // parameters sent

        public bool Success { get; private set; }

        public FullPageMonitorEditer(IRequestSender<SimpleResponse> requestSender, ILog log )
        {
            RequestSender = requestSender;
            Log = log;
            LocationIds = new List<int>();
        }

        public void Edit()
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
                Log.Error("Could not edit monitor", exception);
                Success = false;
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();

            // required parameters 
            data["monitorId"] = MonitorId;
            data["name"] = Name;
            data["tag"] = Tag;
            data["locationIds"] = CreateLocationIdsString();
            data["checkInterval"] = CheckInterval;
            data["url"] = Url;
            data["timeout"] = Timeout;

            // optional parameters
            if (UptimeSla.HasValue)
                data["uptimeSLA"] = UptimeSla.Value;

            if (ResponseSla.HasValue)
                data["responseSLA"] = ResponseSla.Value;

            return data;
        }

        private object CreateLocationIdsString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < LocationIds.Count; i++)
            {
                builder.Append(LocationIds[i]);

                if (i != (LocationIds.Count - 1))
                    builder.Append(",");
            }

            return builder;
        }

        private void ParseResponse(SimpleResponse response)
        {
            Success = (response.Status == "ok");

            int monitorId;
            if (int.TryParse(response.Data, out monitorId))
                MonitorId = monitorId;
        }
    }
}
