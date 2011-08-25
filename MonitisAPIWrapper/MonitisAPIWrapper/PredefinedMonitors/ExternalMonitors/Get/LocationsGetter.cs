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
    public class LocationsGetter
    {
        public IRequestSender<List<LocationsResponse>> RequestSender { get; set; } // send and recieve
        public ILog Log { get; set; } // error log

        public bool Success { get; private set; }
        public List<LocationsResponse> Result { get; private set; }

        public LocationsGetter(IRequestSender<List<LocationsResponse>> requestSender, ILog log)
        {
            RequestSender = requestSender;
            Log = log;
            Result = new List<LocationsResponse>();
        }

        public void Get()
        {
            try
            {
                RequestSender.PostData = new Dictionary<string, object>();
                RequestSender.Send();
                Result = RequestSender.ResponseData;
                Success = true;
            }

            catch (Exception exception)
            {
                Log.Error("Could not get monitor locations", exception);
                Success = false;
            }
        }
    }
}
