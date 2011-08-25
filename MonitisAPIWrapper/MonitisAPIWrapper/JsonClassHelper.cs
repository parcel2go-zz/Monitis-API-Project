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

// JSON C# Class Generator
// http://at-my-window.blogspot.com/?page=json-class-generator

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace JsonCSharpClassGenerator
{
    internal static class JsonClassHelper
    {

        public static T GetJToken<T>(JObject obj, string field) where T : JToken
        {
            JToken value;
            if (obj.TryGetValue(field, out value)) return GetJToken<T>(value);
            else return null;
        }

        private static T GetJToken<T>(JToken token) where T : JToken
        {
            if (token == null) return null;
            if (token.Type == JTokenType.Null) return null;
            if (token.Type == JTokenType.Undefined) return null;
            return (T)token;
        }

        public static string ReadString(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return (string)value.Value;
        }


        public static bool ReadBoolean(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToBoolean(value.Value);

        }

        public static bool? ReadNullableBoolean(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToBoolean(value.Value);
        }


        public static int ReadInteger(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToInt32((long)value.Value);

        }

        public static int? ReadNullableInteger(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToInt32((long)value.Value);

        }



        public static long ReadLong(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToInt64(value.Value);

        }

        public static long? ReadNullableLong(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToInt64(value.Value);
        }


        public static double ReadFloat(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToDouble(value.Value);

        }

        public static double? ReadNullableFloat(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToDouble(value.Value);

        }




        public static DateTime ReadDate(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToDateTime(value.Value);

        }

        public static DateTime? ReadNullableDate(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToDateTime(value.Value);

        }

        public static object ReadObject(JToken token)
        {
            var value = GetJToken<JToken>(token);
            if (value == null) return null;
            if (value.Type == JTokenType.Object) return value;
            if (value.Type == JTokenType.Array) return ReadArray<object>(value, ReadObject);

            var jvalue = value as JValue;
            if (jvalue != null) return jvalue.Value;

            return value;
        }

        public static T ReadStronglyTypedObject<T>(JToken token) where T : class
        {
            var value = GetJToken<JObject>(token);
            if (value == null) return null;
            return (T)Activator.CreateInstance(typeof(T), new object[] { token });

        }


        public delegate T ValueReader<T>(JToken token);



        public static T[] ReadArray<T>(JToken token, ValueReader<T> reader)
        {
            var value = GetJToken<JArray>(token);
            if (value == null) return null;

            var array = new T[value.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = reader(value[i]);
            }
            return array;

        }



        public static Dictionary<string, T> ReadDictionary<T>(JToken token)
        {
            var value = GetJToken<JObject>(token);
            if (value == null) return null;
  
                var dict = new Dictionary<string, T>();

                return dict;
        }

        public static Array ReadArray<K>(JArray jArray, ValueReader<K> reader, Type type)
        {
            if (jArray == null) return null;

            var elemType = type.GetElementType();

            var array = Array.CreateInstance(elemType, jArray.Count);
            for (int i = 0; i < array.Length; i++)
            {
                if (elemType.IsArray)
                {
                    array.SetValue(ReadArray<K>(GetJToken<JArray>(jArray[i]), reader, elemType), i);
                }
                else
                {
                    array.SetValue(reader(jArray[i]), i);
                }

            }
            return array;

        }
    }
}
