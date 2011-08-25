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
using MonitisAPIWrapper.CustomMonitors.Parameters;

namespace MonitisAPIWrapper.CustomMonitors.Post
{
    public class CustomMonitorEditer
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; } // error log

        // parameters sent
        public int MonitorId { get; set; }
        public List<CustomMonitorParam> MonitorParams { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        // parameters sent
        
        public bool Success { get; private set; }

        public CustomMonitorEditer(IRequestSender<SimpleResponse> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
            MonitorParams = new List<CustomMonitorParam>();
        }

        public void Edit()
        {
            try
            {
                RequestSender.PostData = BuildData();
                RequestSender.Send();
                ParseResponse(RequestSender.ResponseData);
            }

            catch (Exception exception)
            {
                Log.Error("Could not edit monitor", exception);
                Success = false;
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<String, Object>();

            data["monitorId"] = MonitorId;

            if (MonitorParams.Count > 0)
                data["monitorParams"] = CreateMonitorParamsString();

            if (!string.IsNullOrEmpty(Name))
                data["name"] = Name;

            if (!string.IsNullOrEmpty(Tag))
                data["tag"] = Tag;

            return data;
        }

        private object CreateMonitorParamsString()
        {
            var builder = new StringBuilder(); 

            foreach (CustomMonitorParam monitorParam in MonitorParams)
            {
                builder.Append(monitorParam.Name);
                builder.Append(":");
                builder.Append(monitorParam.DisplayName);
                builder.Append(":");
                builder.Append(monitorParam.Value);
                builder.Append(":");
                builder.Append((int)monitorParam.DataType);
                builder.Append(":");
            }

            return builder.ToString();
        }

        private void ParseResponse(SimpleResponse simpleResponse)
        {
            Success = (simpleResponse.Status == "ok");
        }
    }
}
