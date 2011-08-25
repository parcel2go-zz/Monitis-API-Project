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
using MonitisAPIWrapper.CustomMonitors.Responses;

namespace MonitisAPIWrapper.CustomMonitors.Get
{
    public class CustomMonitorLister
    {
        public IRequestSender<List<CustomMonitorListResponse>> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; } // error log

        // parameters sent
        public string Tag { get; set; }
        public string Type { get; set; }
        // parameters sent

        public List<CustomMonitorListResponse> Results { get; private set; } // populated with data returned

        public bool Success { get; private set; }

        public CustomMonitorLister(IRequestSender<List<CustomMonitorListResponse>> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
        }

        public void List()
        {
            try
            {
                RequestSender.PostData = BuildData();
                RequestSender.Send();
                Results = RequestSender.ResponseData;
                Success = true;
            }
            catch (Exception exception)
            {
                Log.Error("Could not list monitors", exception);
                Success = false;
            }
        }

        private Dictionary<String, Object> BuildData()
        {
            var data = new Dictionary<String, Object>();

            if (!string.IsNullOrWhiteSpace(Tag))
                data["tag"] = Tag;

            if (!string.IsNullOrWhiteSpace(Type)) 
                data["type"] = Type;

            return data;
        }
    }
}
