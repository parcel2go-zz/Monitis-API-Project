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

namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Post
{
    public class ExternalMonitorDeleter
    {
        public IRequestSender<SimpleResponse> RequestSender { get; set; } // sends and receive 
        public ILog Log { get; set; } // log any errors

        // parameters sent
        public List<int> TestIds { get; set; } 
        // parameters sent

        public bool Success { get; private set; }

        public ExternalMonitorDeleter(IRequestSender<SimpleResponse> requestSender, ILog log )
        {
            RequestSender = requestSender;
            Log = log;
            TestIds = new List<int>();
        }

        public void Delete()
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
                Log.Error("");
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();
            data["testIds"] = CreateTestIdsString();

            return data;
        }

        private object CreateTestIdsString()
        {
            var builder = new StringBuilder();

            foreach (var testId in TestIds)
                builder.Append(testId + ",");

            return builder.ToString();
        }

        private void ParseResponse(SimpleResponse simpleResponse)
        {
            Success = (simpleResponse.Status == "ok");
        }
    }
}
