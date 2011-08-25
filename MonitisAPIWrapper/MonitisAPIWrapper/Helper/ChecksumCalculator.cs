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
using System.Security.Cryptography;
using System.Text;

namespace MonitisAPIWrapper.Helper
{
    /// <summary>
    /// Calculates a checksum based on the parameters of the intended web request which the Monitis API then uses to validate the request.
    /// </summary>
    public class ChecksumCalculator
    {
        public Dictionary<String, Object> Parameters { get; set; }
        public string SecretKey { get; set; }
        public string Checksum { get; private set; }

        public ChecksumCalculator(string secretKey)
        {
            SecretKey = secretKey;
        }

        public void Calculate()
        {
            var builder = new StringBuilder();

            foreach (var keyValuePair in Parameters.OrderBy(x => x.Key))
            {
                builder.Append(keyValuePair.Key);
                builder.Append(keyValuePair.Value);
            }

            Checksum = Encode(builder.ToString());
        }

        public string Encode(string input)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            byte[] keyData = Encoding.UTF8.GetBytes(SecretKey);

            var hmac = new HMACSHA1(keyData);
            byte[] macReciever = hmac.ComputeHash(byteArray);
            return Convert.ToBase64String(macReciever); 
        }
    }
}
