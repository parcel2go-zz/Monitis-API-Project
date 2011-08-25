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
using MonitisAPIWrapper.PredefinedMonitors.FullPageLoadMonitors.Responses;

namespace MonitisAPIWrapper.PredefinedMonitors.FullPageLoadMonitors.Post
{
    public class FullPageMonitorSuspender
    {
        public IRequestSender<FullPageMonitorSuspendResponse> RequestSender { get; set; } // send and recieve 
        public ILog Log { get; set; } // log errors

        // parameters sent
        public List<int> MonitorIds { get; set; } // use if tag is not set
        public string Tag { get; set; } // use if monitor ids is not set
        // parameters sent

        public FullPageMonitorSuspendResponse Result { get; private set; }
        public bool Success { get; private set; }

        public FullPageMonitorSuspender(IRequestSender<FullPageMonitorSuspendResponse> requestSender, ILog log)
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
                Result = RequestSender.ResponseData;
                Success = false;
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

            if (MonitorIds.Count > 0)
                data["monitorIds"] = CreateMonitorIdsString();

            else
                data["tag"] = Tag;

            return data;
        }

        private object CreateMonitorIdsString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < MonitorIds.Count; i++)
            {
                builder.Append(MonitorIds[i]);

                if (i != (MonitorIds.Count - 1))
                    builder.Append(",");
            }

            return builder;
        }
    }
}
