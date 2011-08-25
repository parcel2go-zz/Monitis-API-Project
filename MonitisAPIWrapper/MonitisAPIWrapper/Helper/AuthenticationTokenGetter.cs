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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Common.Logging;
using MonitisAPIWrapper.CustomMonitors.Responses;
using Newtonsoft.Json;

namespace MonitisAPIWrapper.Helper
{
    public class AuthenticationTokenGetter<T>
    {
        public ILog Log { get; set; } // log errors

        private const string URL = "http://www.monitis.com/api";
        private string ApiKey { get; set; }
        private string SecretKey { get; set; }
        private int Version { get; set; }

        public T ResponseData { get; private set; }

        public AuthenticationTokenGetter(string apiKey, string secretKey, int version, ILog log)
        {
            ApiKey = apiKey;
            SecretKey = secretKey;
            Version = version;
            Log = log;
        }

        public void GetToken()
        {
            try
            {
                string parameters = string.Format(@"{0}?action=authToken&apikey={1}&secretkey={2}&version={3}",
                                                  URL, ApiKey, SecretKey, Version);
               var request = WebRequest.Create(parameters);

                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";

                using (Stream responseStream = request.GetResponse().GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    var response = reader.ReadToEnd();
                    Log.Debug("Web response read");
                    ParseResponse(response);
                    Log.Debug("Web response parsed into response data.");
                }
            }

            catch (Exception exception)
            {
                Log.Error("Could not get authCode", exception);
            }
        }

        private void ParseResponse(string response)
        {
            ResponseData = JsonConvert.DeserializeObject<T>(response);
        }
    }
}
