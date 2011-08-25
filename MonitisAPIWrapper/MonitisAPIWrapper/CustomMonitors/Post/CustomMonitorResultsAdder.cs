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
using System.Text;
using Common.Logging;
using MonitisAPIWrapper.CustomMonitors.Parameters;
using MonitisAPIWrapper.Helper;

namespace MonitisAPIWrapper.CustomMonitors.Post
{
    public class CustomMonitorResultsAdder
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; } // error log

        // parameters sent
        public int MonitorId { get; set; }
        public DateTime? CheckTime { get; set; }
        public List<Results> Results { get; set; }
        // parameters sent

        public bool Success { get; private set; }

        public CustomMonitorResultsAdder(IRequestSender<SimpleResponse> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
            Results = new List<Results>();
        }

        public void Add()
        {
            try
            {
                RequestSender.PostData = BuildData();
                RequestSender.Send();
                ParseResponse(RequestSender.ResponseData);
            }

            catch (Exception exception)
            {
                Log.Error("Could not add monitor results", exception);
                Success = false;
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();
            data["monitorId"] = MonitorId;

            if (CheckTime.HasValue)
                data["checktime"] = CheckTime.Value.TicksSinceUnixEpoch();
            else

                data["checktime"] = DateTime.Now.TicksSinceUnixEpoch();

            data["results"] = CreateResultParamsString();

            return data;
        }

        private object CreateResultParamsString()
        {
            var builder = new StringBuilder();
            foreach (Results result in Results)
            {
                builder.Append(result.ParamName);
                builder.Append(":");
                builder.Append(result.ParamValue);
                builder.Append(";");
            }

            return builder.ToString();
        }

        private void ParseResponse(SimpleResponse response)
        {
            Success = (response.Status == "ok");
        }
    }
}
