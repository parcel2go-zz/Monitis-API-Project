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
using MonitisAPIWrapper.CustomMonitors.Responses.MonitorInfo;
using MonitisAPIWrapper.Helper;

namespace MonitisAPIWrapper.CustomMonitors.Get
{
    public class AdditionalCustomMonitorResultsGetter
    {
        public IRequestSender<List<MonitorAdditionalResultResponse>> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; } // log errors

        // parameters sent
        public int MonitorId { get; set; }
        public DateTime? CheckTime { get; set; }
        // parameters sent

        public bool Success { get; private set; }
        public List<MonitorAdditionalResultResponse> Result { get; private set; }

        public AdditionalCustomMonitorResultsGetter(IRequestSender<List<MonitorAdditionalResultResponse>> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
        }

        public void Get()
        {
            try
            {
                RequestSender.PostData = BuildData();
                RequestSender.Send();
                Result = RequestSender.ResponseData;
                Success = true;
            }

            catch (Exception exception)
            {
                Log.Error("Could not get additional monitor results", exception);
                Success = false;
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();

            data["monitorId"] = MonitorId;

            if (CheckTime.HasValue)
                data["checktime"] = CheckTime.Value.TicksSinceUnixEpoch();

            return data;
        }
    }
}
