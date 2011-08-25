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
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Parameters;

namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Post
{
    public class ExternalMonitorEditer
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // sends and receive 
        public ILog Log { get; set; } // log any errors

        // parameters sent
        public int TestId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public List<LocationIds> LocationIds { get; set; }
        public int Timeout { get; set; }
        public string Tag { get; set; }
        public string ContentMatchString { get; set; }
        public int? MaxValue { get; set; }
        public int? UptimeSLA { get; set; }
        public int? ResponseSLA { get; set; }
        // parameters sent

        public bool Success { get; private set; } 

        public ExternalMonitorEditer(IRequestSender<SimpleResponse> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
            LocationIds = new List<LocationIds>();
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
            data["testId"] = TestId;
            data["name"] = Name;
            data["url"] = URL;
            data["locationIds"] = CreateLocationIdsString();
            data["timeout"] = Timeout;
            data["tag"] = Tag;

            // optional parameters
            if (!string.IsNullOrWhiteSpace(ContentMatchString))
                data["contentMatchString"] = ContentMatchString;

            if (MaxValue.HasValue)
                data["maxValue"] = MaxValue;

            if (UptimeSLA.HasValue)
                data["uptimeSLA"] = UptimeSLA;

            if (ResponseSLA.HasValue)
                data["responseSLA"] = ResponseSLA;

            return data;
        }

        private string CreateLocationIdsString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < LocationIds.Count; i++)
            {
                builder.Append(LocationIds[i].LocationId + "-" + (int) LocationIds[i].Interval);

                if (i != (LocationIds.Count - 1))
                    builder.Append(",");
            }

            return builder.ToString();
        }

        private void ParseResponse(SimpleResponse response)
        {
            Success = (response.Status == "ok");
        }
    }
}
