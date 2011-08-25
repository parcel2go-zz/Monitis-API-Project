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
using System.Text;
using Common.Logging;
using MonitisAPIWrapper.CustomMonitors.Parameters;

namespace MonitisAPIWrapper.CustomMonitors.Post
{
    public class CustomMonitorAdder
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; } // log errors

        // parameters sent
        public List<CustomMonitorParam> MonitorParams { get; set; }
        public List<ResultParam> ResultParams { get; set; }
        public List<CustomMonitorParam> AdditionalResultParams { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Type { get; set; }
        // parameters sent

        public bool Success { get; private set; }
        public int? MonitorId { get; private set; } // id returned when monitor is added
        
        public CustomMonitorAdder(IRequestSender<SimpleResponse> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
            MonitorParams = new List<CustomMonitorParam>();
            ResultParams = new List<ResultParam>();
            AdditionalResultParams = new List<CustomMonitorParam>();
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
                Log.Error("Could not add monitor", exception);
                Success = false;
            }
        }

        private Dictionary<String, Object> BuildData()
        {
            var data = new Dictionary<String, Object>();

            if (MonitorParams.Count > 0)
                data["monitorParams"] = CreateMonitorParamsString();

            data["resultParams"] = CreateResultParamsString();

            if (AdditionalResultParams.Count > 0)
                data["additionalResultParams"] = CreateAdditionalResultsParamsString();

            data["name"] = Name;
            data["tag"] = Tag;

            if (!string.IsNullOrEmpty(Type))
                data["type"] = Type;

            return data;
        }

        private string CreateMonitorParamsString()
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
                builder.Append(monitorParam.IsHidden);
                builder.Append(";");
            }

            return builder.ToString();
        }

        private string CreateResultParamsString()
        {
            var builder = new StringBuilder();

            foreach (ResultParam resultParam in ResultParams)
            {
                builder.Append(resultParam.Name);
                builder.Append(":");
                builder.Append(resultParam.DisplayName);
                builder.Append(":");
                builder.Append(resultParam.UnitOfMeasure);
                builder.Append(":");
                builder.Append((int)resultParam.DataType);
                builder.Append(";");
            }

            return builder.ToString();
        }

        private string CreateAdditionalResultsParamsString()
        {
            var builder = new StringBuilder();

            foreach (CustomMonitorParam monitorParam in AdditionalResultParams)
            {
                builder.Append(monitorParam.Name);
                builder.Append(":");
                builder.Append(monitorParam.DisplayName);
                builder.Append(":");
                builder.Append(monitorParam.Value);
                builder.Append(":");
                builder.Append((int)monitorParam.DataType);
                builder.Append(":");
                builder.Append(monitorParam.IsHidden);
                builder.Append(";");
            }

            return builder.ToString();
        }

        private void ParseResponse(SimpleResponse response)
        {
            if (response.Status != "ok")
            {
                Success = false;
            }

            else
            {
                Success = true;
                int monitorId;
                if (int.TryParse(response.Data, out monitorId))
                    MonitorId = monitorId;
            }
        }
    }
}
