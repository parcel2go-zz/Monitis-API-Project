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
using Type = System.Type;

namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Post
{
    public class ExternalMonitorAdder
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // sends and receive 
        public ILog Log { get; set; } // log any errors

        // parameters sent
        public RequestType Type { get; set; }
        public DetailedTestType DetailedTestType { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public Interval Interval { get; set; }
        public int? TimeOut { get; set; }
        public List<int> LocationIds { get; set; }

        public string Tag { get; set; }
        public OverSsl OverSSL { get; set; }
        public string PostData { get; set; }
        public ContentMatchFlag ContentMatchFlag { get; set; }
        public string ContentMatchString { get; set; }

        public Params Params { get; set; }
        public int? UpTimeSla { get; set; }
        public int? ResponseSla { get; set; }
        public string BasicAuthenticationUser { get; set; }
        public string BasicAuthenticationPassword { get; set; }
        // parameters sent

        public bool Success { get; private set; } 
        public int? TestId { get; private set; } // id returned when monitor is added

        public ExternalMonitorAdder(IRequestSender<SimpleResponse> requestSender, ILog log )
        {
            RequestSender = requestSender;
            Log = log;
            LocationIds = new List<int>();
            Params = new Params();
        }

        public void Add()
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
                Log.Error("Could not add monitor", exception);
                Success = false;
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();

            // required parameters
            data["type"] = Type.ToString();
            data["name"] = Name;
            data["url"] = Url;
            data["interval"] = (int)Interval;
            data["locationIds"] = CreateLocationIdsString();
            data["tag"] = Tag;

            // optional parameters
            if (!string.IsNullOrWhiteSpace(DetailedTestType.ToString()))
                data["detailedTestType"] = (int) DetailedTestType;

            if (TimeOut.HasValue)
                data["timeout"] = TimeOut.ToString();

            if (!string.IsNullOrWhiteSpace(OverSSL.ToString()))
                data["overSSL"] = (int) OverSSL;

            if (!string.IsNullOrWhiteSpace(PostData))
                data["postData"] = PostData;

            if (!string.IsNullOrWhiteSpace(ContentMatchFlag.ToString()))
                data["contentMatchFlag"] = (int) ContentMatchFlag;

            if (!string.IsNullOrWhiteSpace(ContentMatchString))
                data["contentMatchString"] = ContentMatchString;

            if (!string.IsNullOrWhiteSpace(CreateParamsString()))
                data["params"] = CreateParamsString();

            if (UpTimeSla.HasValue)
                data["uptimeSLA"] = UpTimeSla;

            if (ResponseSla.HasValue)
                data["responseSLA"] = ResponseSla;

            if (!string.IsNullOrWhiteSpace(BasicAuthenticationUser))
                data["basicAuthUser"] = BasicAuthenticationUser;

            if (!string.IsNullOrWhiteSpace(BasicAuthenticationPassword))
                data["basicAuthPass"] = BasicAuthenticationPassword;

            return data;
        }

        private string CreateLocationIdsString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i <LocationIds.Count; i++)
            {
                if (LocationIds.Count != 1 && i < LocationIds.Count -1)
                    builder.Append(LocationIds[0] + ",");

                else
                    builder.Append(LocationIds[0]);
            }

            return builder.ToString();
        }

        private string CreateParamsString()
        {
            var parameters = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Params.Username))
                parameters.Append("username:" + Params.Username + ";");

            if (!string.IsNullOrWhiteSpace(Params.Password))
                parameters.Append("password:" + Params.Password + ";");

            if (Params.Port.HasValue)
                parameters.Append("port:" + Params.Port + ";");

            if (Params.TimeOut.HasValue)
                parameters.Append("timeout:" + Params.TimeOut + ";");

            if (! string.IsNullOrEmpty(Params.Server))
                parameters.Append("server:" + Params.Server + ";");

            if (! string.IsNullOrWhiteSpace(Params.ExpectedIP))
                parameters.Append("expip:" + Params.ExpectedIP + ";");

            if (!string.IsNullOrWhiteSpace(Params.ExPauth))
                parameters.Append("expauth:" + Params.ExPauth + ";");

            return parameters.ToString();
        }

        private void ParseResponse(SimpleResponse response)
        {
            if (response.Status != "ok")
                Success = false;

            else
            {
                Success = true;
                int monitorId;
                if (int.TryParse(response.Data, out monitorId))
                    TestId = monitorId;
            }
        }
    }
}
