using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace jaytwo.AspNet.SingleSignOn.Utilities
{
    public static class SerializationUtility
    {
        public static string ToJson(object value)
        {
            return new JavaScriptSerializer().Serialize(value);
        }

        public static T FromJson<T>(string json)
        {
            return new JavaScriptSerializer().Deserialize<T>(json);
        }

        public static object FromJson(string json, Type targetType)
        {
            return new JavaScriptSerializer().Deserialize(json, targetType);
        }

        public static IDictionary<string, object> ToDictionary(object value)
        {
            var json = new JavaScriptSerializer().Serialize(value);
            var result = new JavaScriptSerializer().Deserialize<IDictionary<string, object>>(json);
            return result;
        }

        public static object FromDictionary(IDictionary<string, object> dictionary, Type targetType)
        {
            var dictionaryJson = new JavaScriptSerializer().Serialize(dictionary);
            var result = new JavaScriptSerializer().Deserialize(dictionaryJson, targetType);
            return result;
        }

        public static T FromDictionary<T>(IDictionary<string, object> dictionary)
        {
            var dictionaryJson = new JavaScriptSerializer().Serialize(dictionary);
            var result = new JavaScriptSerializer().Deserialize<T>(dictionaryJson);
            return result;
        }
    }
}
