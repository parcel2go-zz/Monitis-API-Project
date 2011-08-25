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
using MonitisAPIWrapper.Helper;
using NUnit.Framework;

namespace MonitisAPIWrapper.Test.HelperTests
{
    [TestFixture]
    public class ChecksumCalculatorTests
    {
        [Test]
        public void CanCalculate()
        {
            var parameters = new Dictionary<String, Object>();
            parameters.Add("name", "simple_custom_monitor");
            parameters.Add("tag", "website position checkers");
            parameters.Add("action", "addMonitor");
            parameters.Add("version", 2);
            parameters.Add("apikey", "5Q1UJD45IM5C21MVOJGIIIREIK");
            parameters.Add("timestamp", "2011-07-13 17:30:00");
            parameters.Add("monitorParams", "search_engine:Search Engine:www.google.com:3:False;");
            parameters.Add("resultParams", "position:Position:N/A:3;");

            var calculator = new ChecksumCalculator("3MLLPEP6KB4F4NH0OCR0VN9RIV") { Parameters = parameters };
            calculator.Calculate();
            Assert.AreEqual(calculator.Checksum, "4K8FWZikUnHJtsbVb9FLXQFo16Q=");
        }

        [Test]
        public void CanEncode()
        {
            var calculator = new ChecksumCalculator("3MLLPEP6KB4F4NH0OCR0VN9RIV");
            var checksum = calculator.Encode("Hello World");
            Assert.AreEqual(checksum, "MvtCt7HhnJWR6P0GmvZb6uB25rk=");
        }

    }
}
