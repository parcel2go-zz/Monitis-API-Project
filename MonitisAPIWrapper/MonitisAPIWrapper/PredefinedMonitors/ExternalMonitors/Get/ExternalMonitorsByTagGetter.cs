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
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Responses;

namespace MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Get
{
    public class ExternalMonitorsByTagGetter
    {
        public IRequestSender<ExternalMonitorsByTagResponse> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; } // log errors

        // parameters sent
        public string Tag { get; set; }
        // parameters sent

        public ExternalMonitorsByTagResponse Result { get; private set; }
        public bool Success { get; private set; }

        public ExternalMonitorsByTagGetter(IRequestSender<ExternalMonitorsByTagResponse> requestSender, ILog log )
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
                Success = false;
                Log.Error("Could not get monitors by tag", exception);
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();
            data["tag"] = Tag;

            return data;
        }
    }
}
