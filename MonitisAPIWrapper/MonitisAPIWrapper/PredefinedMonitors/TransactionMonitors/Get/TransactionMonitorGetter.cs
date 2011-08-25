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
using MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Parameters;

namespace MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Get
{
    public class TransactionMonitorGetter
    {
        public IRequestSender<TransactionMonitorResponse> RequestSender { get; set; }
        public ILog Log { get; set; }

        // parameters sent
        public TransactionMonitorType Type { get; set; }
        // parameters sent

        public Boolean Success { get; private set; }
        public TransactionMonitorResponse Result { get; private set; }

        public TransactionMonitorGetter(IRequestSender<TransactionMonitorResponse> requestSender, ILog log)
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
                Log.Error("Couldn't get monitors", exception);
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();

            data["type"] = (int)Type;

            return data;
        }
    }
}
