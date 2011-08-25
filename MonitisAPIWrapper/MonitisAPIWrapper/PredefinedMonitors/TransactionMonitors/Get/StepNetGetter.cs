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
    public class StepNetGetter
    {
        public IRequestSender<StepNetResponse> RequestSender { get; set; } // send and receive
        public ILog Log { get; set; } // log errors

        // parameters sent
        public int ResultId { get; set; }
        public DateTime Date { get; set; }
        // parameters sent

        public bool Succeess { get; private set; }
        public StepNetResponse Result { get; private set; }

        public StepNetGetter(IRequestSender<StepNetResponse> requestSender, ILog log)
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
                Succeess = true;
            }

            catch(Exception exception)
            {
                Succeess = false;
                Log.Error("Couldn't get step net", exception);
            }
        }

        private Dictionary<string, object> BuildData()
        {
            var data = new Dictionary<string, object>();
            data["resultId"] = ResultId;
            data["year"] = Date.Year;
            data["month"] = Date.Month;
            data["day"] = Date.Day;

            return data;
        }
    }
}
 