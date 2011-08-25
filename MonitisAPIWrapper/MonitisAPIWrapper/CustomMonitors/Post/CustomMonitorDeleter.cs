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
using Common.Logging;

namespace MonitisAPIWrapper.CustomMonitors.Post
{
    public class CustomMonitorDeleter
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; } // error log

        // parameters sent
        public int MonitorId { get; set; } 
        // parameters sent
        
        public bool Success { get; private set; }

        public CustomMonitorDeleter(IRequestSender<SimpleResponse> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
        }

        public void Delete()
        {
            try
            {
                RequestSender.PostData = BuildData();
                RequestSender.Send();
                ParseResponse(RequestSender.ResponseData);
            }
            catch (Exception exception)
            {
                Log.Error("Could not delete monitor", exception);
                Success = false;
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();
            data["monitorId"] = MonitorId;
            return data;
        }

        private void ParseResponse(SimpleResponse response)
        {
            Success = (response.Status == "ok");
        }
    }
}
