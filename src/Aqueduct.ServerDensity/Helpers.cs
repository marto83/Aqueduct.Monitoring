using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Web;

namespace Aqueduct.ServerDensity
{
    public static class Helpers
    {
        /// <summary>
        /// Constructs a NameValueCollection into a query string.
        /// </summary>
        /// <remarks>Consider this method to be the opposite of "System.Web.HttpUtility.ParseQueryString"</remarks>
        /// <param name="parameters">The NameValueCollection</param>
        /// <param name="delimiter">The String to delimit the key/value pairs</param>
        /// <returns>A key/value structured query string, delimited by the specified String</returns>
        public static string ToQueryString(this NameValueCollection parameters, String delimiter = "&", Boolean omitEmpty = true)
        {
            if (parameters == null)
                return string.Empty;

            if (String.IsNullOrEmpty(delimiter))
                delimiter = "&";

            Char equals = '=';
            List<String> items = new List<String>();

            for (int i = 0; i < parameters.Count; i++)
            {
                foreach (String value in parameters.GetValues(i))
                {
                    Boolean addValue = (omitEmpty) ? !String.IsNullOrEmpty(value) : true;
                    if (addValue)
                        items.Add(String.Concat(parameters.GetKey(i), equals, HttpUtility.UrlEncode(value)));
                }
            }

            return String.Join(delimiter, items.ToArray());
        }
    }
}
