using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.Domain.Data
{
    public static class DataUtility
    {
        public static async Task<Expression<T>> IncludeExpression<T>(this Type domainType, string includeStr)
        {
            var options = ScriptOptions.Default.AddReferences(domainType.Assembly)
                .AddImports("System.Collections")
                .AddImports("System.Collections.Generic")
                .AddImports("System.Linq");

            var includeExpression = await CSharpScript.EvaluateAsync<Expression<T>>(includeStr, options);

            return includeExpression;
        }

        public static NameValueCollection ToNameValueCollection<T>(this T data) where T : class
        {
            NameValueCollection result = new NameValueCollection();

            data.GetType().GetProperties()
                .ToList()
                .ForEach(pi =>
                {
                    var value = pi.GetValue(data, null)?.ToString();

                    result.Add(pi.Name, value);
                });

            return result;
        }

        public static List<KeyValuePair<string, DataTable>> TablesToDictionary(this DataTableCollection tables)
        {

            var dic = new List<KeyValuePair<string, DataTable>>();

            for (var i = 0; i < tables.Count; i++)
            {
                // tables[i].TableName 規定需一定要是 BU 代碼
                dic.Add(new KeyValuePair<string, DataTable>(tables[i].TableName, tables[i]));
            }

            return dic;
        }

        public static T Clone<T>(this T obj)
        {
            var value = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));

            return value;
        }

        public static string ParsingTemplate<T>(this T Passive, string template)
        {

            if (string.IsNullOrEmpty(template)) throw new Exception("[Method] ParsingTemplate Template 參數為空");

            // 擷取 {{文字}} 的資料
            //var pattern = "({{.*}})";

            // 預設 Template 樣式
            StringBuilder sb = new StringBuilder(template);

            // 取得 ReplaceAttribute 的屬性
            //var _props = RecursiveProperty(Passive.GetType());
            var _props = Passive?.GetType()?.GetProperties()?.Where(p => p.IsDefined(typeof(ReplaceAttribute))).ToList();

            // 取得符合格式的字串
            //var matches = Regex.Matches(template, pattern);
            var matches = template.Matchs((empty) => new Regex(@"{{(\w+)}}"));


            foreach (string match in matches)
            {
                // 去掉 '{{' and '}}' 取出 KEY
                var name = match.Replace("{{", "").Replace("}}", "");

                // 取得符合 KEY 的欄位
                var prop = _props.Where(x =>
                {
                    return x.GetCustomAttributes<ReplaceAttribute>().Where(g => g.Key == name).Count() > 0;
                })
                                  .FirstOrDefault();

                var value = prop?.GetValue(Passive);

                var attr = prop?.GetCustomAttributes<ReplaceAttribute>().FirstOrDefault(x => x.Key == name);

                if (value == null) continue;

                // 如有指定特定 Parser 就走該 Parser
                if (attr.Parser != null)
                {
                    var instance = Activator.CreateInstance((Type)attr.Parser);
                    var method = ((Type)attr.Parser).GetMethod(attr.MethodName);
                    var result = method.Invoke(
                                    instance,
                                    BindingFlags.Public | BindingFlags.InvokeMethod,
                                    null,
                                    new object[] { value },
                                    null);

                    // 回傳 null 時不清除 template tag
                    if (IsNull(result) == false)
                        sb.Replace(match, result.ToString());
                    

                }
                else
                {
                    sb.Replace(match, value.ToString());
                }
            }

            return sb.ToString();
        }


        public static Object GetValueFromProp(this object obj, string prop)
        {

            return obj.GetType()
                      .GetProperty(prop)
                      .GetValue(obj, null);
        }

        private static bool FilterGuard(PropertyInfo Property, object Value)
        {

            if (Value == null) return false;

            if (Value is string && string.IsNullOrEmpty((string)Value)) return false;


            return true;
        }

        public static List<T> GetAttribute<T>(this object @object) where T : BaseAttribute
        {

            List<T> filters = new List<T>();

            var properties = @object.GetType()
                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .AsEnumerable();

            properties.ToList()
                  .ForEach(prop =>
                  {

                      var value = @object.GetValueFromProp(prop.Name);

                      if (FilterGuard(prop, value) == false) return;

                      var filter = prop.GetCustomAttribute<T>();

                      if (filter != null)
                      {

                          filter.Value = value;
                          filters.Add(filter);
                      }
                  });

            return filters;

        }

        public static int[] TryGetBuList<OrganizationType>(this IDictionary<OrganizationType, int[]> index, OrganizationType key, OrganizationType nonExistMappedKey = default(OrganizationType))
        {
            if (key == null)
                return new int[] { };

            if (index.TryGetValue(key, out var service))
            {
                return service;
            }

            if (nonExistMappedKey.Equals(default(OrganizationType)))
                return new int[] { };

            return index[nonExistMappedKey];
        }

        public static string GetTableConcataAndComplained(this ConcatableUser user)
        {
            if (user == null)
                return string.Empty;
            switch (user.UnitType)
            {

                case UnitType.Customer:
                    return user.UserName + user.Gender.GetDescription();

                case UnitType.Store:
                    return user.NodeName + user.StoreNo;

                case UnitType.Organization:
                    return user.NodeName;

                default:
                    return string.Empty;
            }
        }

        
        #region 差異比對 & 物件轉文字

        /// <summary>
        /// 將物件內容解析為文字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Passive"></param>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public static string GetObjectContentFromDescription<T>(this T Passive, string Prefix = "")
        {
            if (Passive == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            try
            {
                var props = Passive?.GetType()?.GetProperties()?.ToList();

                props?.ForEach(p =>
                {
                    try
                    {
                        var attr = p.GetCustomAttribute<DescriptionAttribute>();

                        if (attr == null) return;

                        var value = p.GetValue(Passive);

                        if (value == null)
                        {
                            sb.Append($"【{Prefix}{attr.Description}】: 無資訊");
                            sb.Append(Environment.NewLine);

                            return;
                        }


                        if (value.GetType().IsGenericType &&
                            value.GetType()?.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            dynamic payload = value;
                            sb.Append($"【{Prefix}{attr.Description}】: 複雜型別-------------------------");
                            sb.Append(Environment.NewLine);
                            var i = 1;
                            foreach (var item in payload)
                            {

                                sb.Append($"【{Prefix}{attr.Description}】- {i}");
                                sb.Append(Environment.NewLine);

                                sb.Append(GetObjectContentFromDescription(item, Prefix + "-" + attr.Description));
                                sb.Append(Environment.NewLine);
                                i = i + 1;
                            }
                        }

                        if (IsClass(value.GetType()))
                        {
                            sb.Append(GetObjectContentFromDescription(value, Prefix + "-" + attr.Description));
                            sb.Append(Environment.NewLine);

                        }
                        else
                        {

                            sb.Append($"【{Prefix}{attr.Description}】:{ParsingDiffFormat(value, p)}");
                            sb.Append(Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.Append($"【ERROR{Prefix}】: {ex}");
                        sb.Append(Environment.NewLine);
                    }

                });

            }
            catch (Exception ex)
            {

            }

            return sb.ToString();
        }

        /// <summary>
        /// 比較兩者不同的屬性 , 並印出值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Passive"></param>
        /// <param name="Initiative"></param>
        /// <returns></returns>
        public static string CustomerDiffFormatter<T>(this T Passive, T Initiative, string Prefix = "")
        {
            StringBuilder sb = new StringBuilder();

            var props = Passive?.GetType()?.GetProperties()?.ToList();

            props?.ForEach(p =>
            {

                var attr = p.GetCustomAttribute<CustomAttribute>();

                if (attr == null) return;

                var pv = Passive != null ? p.GetValue(Passive) : null;
                var pi = Initiative != null ? p.GetValue(Initiative) : null;

                // 兩者都為null , 就回傳
                if (pv == null && pi == null) return;


                if (pv == null || pi == null)
                {

                    // 如果傳入值不為空 , 就轉format
                    if (pi == null)
                    {
                        sb.Append($"【{Prefix}{attr.Description}】異動為 無 \r\n");
                        sb.AppendLine();
                        return;
                    }

                    if (IsClass(pi.GetType()))
                    {
                        sb.Append(CustomerDiffFormatter(pv, pi, Prefix + attr.Description));


                    }
                    else
                    {
                        sb.Append($"【{Prefix}{attr.Description}】{ParsingDiffFormat(pv, p)} 異動為 {ParsingDiffFormat(pi, p)}  \r\n");
                        sb.AppendLine();
                    }

                    return;
                }

                if (pv.GetType().IsGenericType && pv.GetType()?.GetGenericTypeDefinition() == typeof(List<>) && pi.GetType().IsGenericType && pi.GetType()?.GetGenericTypeDefinition() == typeof(List<>))
                {
                    dynamic payloadv = pv;
                    dynamic payloadi = pi;
                    var i = 1;
                    foreach (var item in payloadv)
                    {
                        sb.Append(CustomerDiffFormatter(item, payloadi[i-1], Prefix + attr.Description));
                        sb.Append(Environment.NewLine);
                        i = i + 1;
                    }
                    return;
                }

                if (IsClass(pv.GetType()))
                {
                    sb.Append(CustomerDiffFormatter(pv, pi, Prefix + attr.Description));
                    return;
                }

                if (!pv.Equals(pi))
                {
                    sb.Append($"【{Prefix}{attr.Description}】{ParsingDiffFormat(pv, p)} 異動為 {ParsingDiffFormat(pi, p)}  \r\n");
                    sb.AppendLine();
                }



            });

            return sb.ToString();


        }

        /// <summary>
        /// 是否為類別
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsClass(Type type)
        {
            return type.IsClass && type != typeof(string);
        }

        /// <summary>
        /// 是否為數值型態
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsSimple(Type type)
        {
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        /// <summary>
        /// 解析內文
        /// </summary>
        /// <param name="Obj"></param>
        public static string ParsingDiffFormat(object Obj, PropertyInfo Property)
        {
            if (Obj == null) return "無";

            Type type = Obj.GetType();

            if (type.IsEnum)
            {
                var attr = Obj.GetDescription();

                return Obj.GetDescription();
            }
            else if (IsNumericType(type))
            {
                return Obj.ToString();

            }
            else if (type == typeof(string))
            {
                return Obj.ToString();
            }
            else if (type == typeof(bool))
            {
                return ((Boolean)Obj) ? "是" : "否";
            }
            else if (type == typeof(DateTime))
            {
                var formatAttr = Property.GetCustomAttribute<DateTimeFormatAttribute>();

                if (formatAttr == null)
                {
                    return Obj.ToString();
                }
                else
                {

                    var rangeAttr = Property.GetCustomAttribute<DateTimeRangeAttribute>();
                    var operatorAttr = Property.GetCustomAttribute<DateTimeOperatorAttribute>();

                    if (rangeAttr != null)
                    {

                        var start = (Obj as DateTime?).Value;
                        var end = (operatorAttr == null) ? start : CalcDateTime(start, operatorAttr.Value, operatorAttr.Type);

                        return $"{start.ToString(formatAttr.Format)}-{end.ToString(formatAttr.Format)}";
                    }
                    else
                    {
                        var start = (Obj as DateTime?).Value;
                        var result = (operatorAttr == null) ? start : CalcDateTime(start, operatorAttr.Value, operatorAttr.Type);
                        return result.ToString(formatAttr.Format);
                    }


                }
            }


            return Obj.ToString();
        }

        /// <summary>
        /// 計算截止時間
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static DateTime CalcDateTime(DateTime DateTime, Double Value, DateTimeOperatorType Type)
        {
            switch (Type)
            {
                case DateTimeOperatorType.Milliseconds:
                    return DateTime.AddMilliseconds(Value);
                case DateTimeOperatorType.Seconds:
                    return DateTime.AddSeconds(Value);
                case DateTimeOperatorType.Minutes:
                    return DateTime.AddMinutes(Value);
                case DateTimeOperatorType.Hours:
                    return DateTime.AddHours(Value);
                case DateTimeOperatorType.Days:
                    return DateTime.AddDays(Value);
                case DateTimeOperatorType.Months:
                    return DateTime.AddMonths((int)Value);
                case DateTimeOperatorType.Years:
                    return DateTime.AddYears((int)Value);
                default:
                    throw new Exception("Datetime 計算內容錯誤");
            }
        }

        #endregion

        /// <summary>
        /// 建立IGrouping物件
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        public class Grouping<TKey, TElement> : List<TElement>, IGrouping<TKey, TElement>
        {
            public Grouping(TKey key) : base() => Key = key;
            public Grouping(TKey key, int capacity) : base(capacity) => Key = key;
            public Grouping(TKey key, IEnumerable<TElement> collection)
                : base(collection) => Key = key;
            public TKey Key { get; private set; }
        }

        private static bool IsNull(object data)
        {

            if (data is String)
            {
                return string.IsNullOrEmpty((string)data);
            }
            else
            {
                return data == null;
            }

        }
    }

    

}
