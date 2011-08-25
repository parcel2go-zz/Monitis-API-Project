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
using System.Net;
using System.Text;
using System.Web;
using Common.Logging;
using MonitisAPIWrapper.CustomMonitors.Get;
using MonitisAPIWrapper.CustomMonitors.Responses;
using MonitisAPIWrapper.Helper;
using Newtonsoft.Json;

namespace MonitisAPIWrapper
{
    /// <summary>
    /// Sends a POST web request to the Monitis API
    /// </summary>
    /// <typeparam name="T">The type of web response</typeparam>
    public class PostRequestSender<T> : IRequestSender<T>
    {
        public string URL { get; set; }
        public string Action { get; set; }
        public string Apikey { get; set; }
        public int Version { get; set; }
        public OutputType Output { get; set; }
        public Validation Validation { get; set; }

        public ChecksumCalculator ChecksumCalculator { get; set; }
        public AuthenticationTokenGetter<AuthenticationTokenResponse> AuthTokenGetter { get; set; }
        public Dictionary<string, object> PostData { get; set; }
        public T ResponseData { get; private set; }
        public ILog Log { get; set; }
        
        private string authToken { get; set; }
        public string AuthToken
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(authToken))
                    return authToken;
                
                    return GetAuthToken();
            }

            set { authToken = value; }
        }

        public PostRequestSender(string url, string action, string apikey, ChecksumCalculator checksumCalculator, AuthenticationTokenGetter<AuthenticationTokenResponse> authTokenGetter, int version, ILog log)
        {
            URL = url;
            Action = action;
            Apikey = apikey;
            ChecksumCalculator = checksumCalculator;
            AuthTokenGetter = authTokenGetter;
            Version = version;
            Log = log;
        }

        private string GetAuthToken()
        {
            if (string.IsNullOrWhiteSpace(Apikey))
                return "";

            AuthTokenGetter.GetToken();
            var response = AuthTokenGetter.ResponseData;

            if (response != null && string.IsNullOrWhiteSpace(response.Error) && ! string.IsNullOrWhiteSpace(response.AuthToken))
                return response.AuthToken;

            return "";
        }

        public void Send()
        {
            byte[] buffer = AggregateAllPostData();

            var request = WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;
            
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
                Log.Debug("Web request generated");
            }
            
            using (Stream responseStream = request.GetResponse().GetResponseStream())
            {
                var reader = new StreamReader(responseStream);
                var response = reader.ReadToEnd();
                Log.Debug("Web response read");
                ParseResponse(response);
                Log.Debug("Web response parsed into response data.");
            }
        }

        private byte[] AggregateAllPostData()
        {
            var allPostData = new Dictionary<string, object>(PostData)
                                  {
                                      {"action", Action},
                                      {"version", Version},
                                      {"apikey", Apikey},
                                      {"timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
                                  };

            if (! string.IsNullOrWhiteSpace(Output.ToString()))
                allPostData["output"] = Output;

            if (! string.IsNullOrWhiteSpace(Validation.ToString()))
                allPostData["validation"] = Validation.ToString();

            SetPostDataValidation(allPostData);
            return EncodePostData(allPostData);
        }

        private void SetPostDataValidation(Dictionary<string, object> allPostData)
        {
            if (!string.IsNullOrWhiteSpace(Validation.ToString()) && Validation == Validation.token)
                allPostData["authToken"] = AuthToken;

            else
            {
                ChecksumCalculator.Parameters = allPostData;
                ChecksumCalculator.Calculate();
                allPostData.Add("checksum", ChecksumCalculator.Checksum);
            }
        }

        private static byte[] EncodePostData(Dictionary<string, object> postData)
        {
            var builder = new StringBuilder();

            foreach (var pair in postData)
            {
                builder.Append(HttpUtility.UrlEncode(pair.Key));
                builder.Append("=");
                builder.Append(HttpUtility.UrlEncode(pair.Value.ToString()));
                builder.Append("&");
            }

            var escapedData = builder.ToString().TrimEnd('&');
            return Encoding.ASCII.GetBytes(escapedData);
        }

        private void ParseResponse(string response)
        {
            ResponseData = JsonConvert.DeserializeObject<T>(response);
        }
    }
}
