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
using MonitisAPIWrapper.Helper;
using Newtonsoft.Json;

namespace MonitisAPIWrapper.CustomMonitors.Post
{
    public class AdditonalCustomMonitorResultsAdder
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; }  // error log

        // parameters sent
        public int MonitorId { get; set; }
        public DateTime? CheckTime { get; set; }
        public List<Results> Results { get; set; }
        // parameters sent

        public bool Success { get; private set; }

        public AdditonalCustomMonitorResultsAdder(IRequestSender<SimpleResponse> sender, ILog log)
        {
            RequestSender = sender;
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

            catch(Exception exception)
            {
                Log.Error("Could not add additional monitor results", exception);
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
            var jsonArray = JsonConvert.SerializeObject(Results);
            return jsonArray;
        }

        private void ParseResponse(SimpleResponse response)
        {
            Success = (response.Status == "ok");
        }
    }
}
