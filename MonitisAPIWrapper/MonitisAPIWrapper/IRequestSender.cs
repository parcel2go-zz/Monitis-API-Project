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

using System.Collections.Generic;
using MonitisAPIWrapper.CustomMonitors.Responses;
using MonitisAPIWrapper.Helper;

namespace MonitisAPIWrapper
{
    /// <summary>
    /// Sends a web request to the Monitis API
    /// </summary>
    /// <typeparam name="T">The type of web response</typeparam>
    public interface IRequestSender<T>
    {
        // properties
        string URL { get; set; }
        string Action { get; set; }
        string Apikey { get; set; }
        int Version { get; set; }
        
        // newly added
        OutputType Output { get; set; }
        Validation Validation { get; set; }
        AuthenticationTokenGetter<AuthenticationTokenResponse> AuthTokenGetter {get; set;} 
        // newly added

        ChecksumCalculator ChecksumCalculator { get; set; }
        Dictionary<string, object> PostData { get; set; }
        T ResponseData { get; }

        // methods
        void Send();
    }
}