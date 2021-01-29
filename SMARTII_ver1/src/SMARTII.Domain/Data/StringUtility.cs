using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using MoreLinq;
using Newtonsoft.Json;
using SMARTII.Domain.Cache;

namespace SMARTII.Domain.Data
{
    public static class StringUtility
    {
        public static byte[] GetBytes(this string input)
        {
            byte[] numArray = new byte[input.Length * 2];
            Buffer.BlockCopy((Array)input.ToCharArray(), 0, (Array)numArray, 0, numArray.Length);
            return numArray;
        }

        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType)
        {
            if (input != null && suffixToRemove != null
              && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            else return input;
        }

        public static string ConcatArray(this string[] input, string split = "", StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            var result = string.Empty;

            input?.ForEach(x => result += $"{x}{split}");

            return result.TrimEnd(split, stringComparison);
        }

        public static string CombineStr(this string[] inputs, string prefix = "")
        {
            var result = string.Empty;

            result = string.Join(prefix, inputs);

            return result;
        }

        public static string NestedLambdaString(this string lambdaSytax, string body, string prefix = "", int depth = 1)
        {
            int index = 1;
            while (depth >= index)
            {
                var _prefix = (Convert.ToChar(65 + (index / 25))).ToString();
                var _suffix = index % 26;
                var key = $"{_prefix}{_suffix}";
                prefix += $".{lambdaSytax}({key} => {key}.{body}";
                if (depth == index)
                {
                    prefix += new String(')', index);
                }
                index++;
            }
            return prefix;
        }

        public static string NestedLambdaStringToUp(this string lambdaSytax, string body, string prefix = "", int depth = 1)
        {
            int index = 1;
            while (depth >= index)
            {
                var _prefix = (Convert.ToChar(65 + (index / 25))).ToString();
                var _suffix = index % 26;
                var key = $"{_prefix}{_suffix}";
                prefix += $".{body}";
                index++;
            }
            return prefix;
        }

        public static string ToQueryString(this NameValueCollection data)
        {
            var list = new List<string>();
            foreach (var key in data.AllKeys)
            {
                var value = data.GetValues(key)?.FirstOrDefault();

                if (string.IsNullOrEmpty(value) == false)
                {
                    list.Add($"{ HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}");
                }
            }

            return "?" + string.Join("&", list.ToArray());
        }

        public static string ToDateRangePicker(this DateTime? start, DateTime? end, string format = "yyyy/MM/dd HH:mm:ss")
        {
            var startStr = start.HasValue ? start.Value.ToString(format) : "";
            var endStr = end.HasValue ? end.Value.ToString(format) : "";

            return (!String.IsNullOrEmpty(startStr) && !String.IsNullOrEmpty(endStr)) ? startStr + " - " + endStr : "";
        }

        public static DateTime? EndTime(this string DateTimeRange)
        {
            return String.IsNullOrEmpty(DateTimeRange) ?
                default(DateTime?) :
                Convert.ToDateTime(DateTimeRange.Split('-')[1].Trim());
        }
        /// <summary>
        /// 期望期限開始時間
        /// </summary>
        public static DateTime? StarTime(this string DateTimeRange)
        {
            return String.IsNullOrEmpty(DateTimeRange) ?
                default(DateTime?) :
                Convert.ToDateTime(DateTimeRange.Split('-')[0].Trim());
        }

        public static string InsertArraySerialize(this string existArrayStr, string[] newArray)
        {
            if (string.IsNullOrEmpty(existArrayStr))
            {
                return JsonConvert.SerializeObject(newArray);
            }
            else
            {
                var existPath = JsonConvert.DeserializeObject<List<string>>(existArrayStr);

                if (newArray != null)
                    existPath.AddRange(newArray);

                return JsonConvert.SerializeObject(existPath);
            }
        }

        public static string RemoveArraySerialize(this string existArrayStr, string key)
        {
            if (string.IsNullOrEmpty(existArrayStr) == false)
            {
                var existPath = JsonConvert.DeserializeObject<List<string>>(existArrayStr);

                existPath = existPath.Where(c => c.Contains(key) == false)
                                     .ToList();

                return JsonConvert.SerializeObject(existPath);
            }

            return JsonConvert.SerializeObject(new string[] { });
        }

        public static List<string> SpecificString(this string temp, string leftKey, string rightKey, bool exclude = true)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(temp))
            {

                var _temp = temp;
                var _leftKey = leftKey.Replace(@"\", "");
                var _rightKey = rightKey.Replace(@"\", "");
                //var pattern = $"{leftKey}" + @"(.+)" + $"{rightKey}";//20201013 此寫法會導致有多個相同標籤時只會擷取一筆(第一筆符合的開頭+最後一筆符合的結尾中 全部字串)
                var pattern = $"{leftKey}" + "[^"+ rightKey.Replace("]", @"\]") + "]+" + $"{rightKey}";//20201013 新增排除規則

                var values = StringUtility.Matchs(temp, (empty) => new Regex(pattern));
                
                values = exclude ? values.Select(x => x.Replace(_leftKey, "").Replace(_rightKey, "")).ToList() : values;

                result.AddRange(values);
                
            }


            return result;
        }

        public static List<string> Matchs(this string temp, Func<object, Regex> action)
        {

            var rgx = action(string.Empty);

            var matchs = rgx.Matches(temp);

            var values = matchs?
                            .Cast<Match>()?
                            .Select(m => m.Value)
                            .ToList() ?? new List<string>();

            return values;
        }

        public static string Match(this string temp, Func<object, Regex> action)
        {

            var rgx = action(string.Empty);

            var match = rgx.Match(temp);
            
            return match.Success ? match.Value : string.Empty;
        }

        public static bool ContainsIfEmpty(this string str, string compare)
        {
            if (string.IsNullOrEmpty(compare) || string.IsNullOrEmpty(str))
            {
                return false;
            }

            return str.Contains(compare);
        }


        public static string Encoding(this string str)
        {
            return HttpUtility.UrlEncode(str).Replace("+", "%20");
        }

        public static string ToHtmlFormat(this string content)
        {
            return Regex.Replace(content, @"\r\n?|\n", "<br>");
        }
        
        public static string GetComplaintInvoiceName(this string dynamicName, string fileType, string nodeKey)
        {
            var name = string.Empty;

            if (EssentialCache.BusinessKeyValue._21Century == nodeKey)
            {
                name = $"21世紀顧客反應單-{dynamicName}-S5";
            }
            else if (EssentialCache.BusinessKeyValue.MisterDonut == nodeKey)
            {
                name = $"多拿滋顧客反應單-{dynamicName}-S5";
            }
            else if (EssentialCache.BusinessKeyValue.ColdStone == nodeKey)
            {
                name = $"酷聖石顧客反應單-{dynamicName}-S5";
            }
            else if (EssentialCache.BusinessKeyValue.PPCLIFE == nodeKey)
            {
                name = $"統一藥品重大客訴案件處理單-{dynamicName}-S5";
            }


            return $"{name}.{fileType}";
        }

    }
}
