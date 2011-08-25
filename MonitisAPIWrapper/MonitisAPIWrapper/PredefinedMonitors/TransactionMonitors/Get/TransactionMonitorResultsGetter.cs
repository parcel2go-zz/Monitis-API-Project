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
using MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Response;
using Common.Logging;

namespace MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Get
{
    public class TransactionMonitorResultsGetter
    {
        public IRequestSender<List<TransactionMonitorResultsResponse>> RequestSender {get;set;} // send and receive
        public ILog Log { get; set; } // log errors

        // parameters sent
        public int MonitorId { get; set; }
        public List<int> LocationIds { get; set; }
        public DateTime Date { get; set; }
        public int? TimeZone { get; set; }
        // parameters sent

        public bool Successful { get; private set; }
        public List<TransactionMonitorResultsResponse> Result { get; private set; }

        public TransactionMonitorResultsGetter(IRequestSender<List<TransactionMonitorResultsResponse>> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
            LocationIds = new List<int>();
        }

        public void Get()
        {
            try
            {
                RequestSender.PostData = BuildData();
                RequestSender.Send();
                Result = RequestSender.ResponseData;
                Successful = true;
            }

            catch (Exception exception)
            {
                Successful = false;
                Log.Error("Could not get monitor results", exception);
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();

            // required parameters 
            data["monitorId"] = MonitorId;
            data["year"] = Date.Year;
            data["month"] = Date.Month;
            data["day"] = Date.Day;

            // optional parameters 
            if (TimeZone.HasValue)
                data["timezone"] = TimeZone.Value;

            if (LocationIds.Count > 0)
                data["locationIds"] = CreateLocationIdsString();

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
    }
}
