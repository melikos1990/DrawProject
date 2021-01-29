using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Data
{
    public static class DisplayUtility
    {
        public static string DisplayWhenNull(this ResultPropertyCollection keyValue,
            string key,
            int index = 0,
            string text = "")
        {
            return keyValue?[key].Count > 0 ? keyValue[key][index].ToString() : text;
        }

        public static string DisplayWhenNull(this DateTime? dateTime,
                                    string formatter = "yyyy/MM/dd HH:mm:ss",
                                    string text = "無")
        {
            if (!dateTime.HasValue)
                return text;
            else
                return dateTime.Value.ToString(formatter);
        }

        public static string DisplayWhenNull(this string value,
                                        string text = "無")
        {
            if (string.IsNullOrEmpty(value))
                return text;
            else
                return value;
        }

        public static string DisplayWhenNull(this int? value,
                                      string text = "無")
        {
            if (!value.HasValue)
                return text;
            else
                return value.ToString();
        }

        public static string DisplayBit(this Boolean flag, string @true = "是", string @false = "否")
        {
            return flag ? @true : @false;
        }

        public static string[] ParseEnumFlags<T>(this T @enum) where T : struct
        {
            return
            @enum
            .ToString()
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(
                strenum =>
                {
                    T outenum;
                    Enum.TryParse(strenum, out outenum);
                    return outenum.GetDescription();
                })
            .ToArray();
        }

        public static string DisplayFromHttpParamter(this IDictionary<string, object> dist)
        {
            if (dist == null)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var keyValue in dist)
            {
                if (keyValue.Value == null)
                {
                    //sb.Append($"KEY : {keyValue.Key}");
                    continue;
                }

                if (keyValue.Value.GetType() == typeof(HttpPostedFileWrapper))
                {
                    //sb.Append($"KEY : {keyValue.Key} , VALUE  : 檔案格式不支援輸出");
                    continue;
                }

                string jsonContent = JsonConvert.SerializeObject(keyValue.Value);
                sb.Append(jsonContent);
            }

            return sb.ToString();
        }

        public static string GetDescription<T>(this T EnumValue)
        {
            if (EnumValue == null)
                return "";

            var type = EnumValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(EnumValue)} must be of Enum type", nameof(EnumValue));
            }
            var memberInfo = type.GetMember(EnumValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return EnumValue.ToString();
        }

        public static T GetFromDescription<T>(this string EnumName)
        {
            Type type = typeof(T);

            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(type)} must be of Enum type", nameof(type));
            }

            var memberInfoList = type.GetMembers();

            foreach (var memberInfo in memberInfoList)
            {
                var attrs = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    var attr = (DescriptionAttribute)attrs[0];

                    if (attr.Description == EnumName)
                        return (T)Enum.Parse(typeof(T), memberInfo.Name);
                }
            }

            return default(T);
        }

        public static T GetEnumFromString<T>(this string enumString)
        {
            if (typeof(T).IsEnum)
            {
                T enumType = (T)Enum.Parse(typeof(T), enumString);

                return enumType;
            }
            return default(T);
        }

        public static T GetEnumFromKeyString<T>(this string enumString)
        {
            if (typeof(T).IsEnum)
            {
                if (string.IsNullOrEmpty(enumString)) return default(T);

                if (int.TryParse(enumString, out int result))
                {
                    T target = (T)Enum.ToObject(typeof(T), result);

                    return target;
                }
                else
                {
                    return default(T);
                }
            }
            return default(T);
        }

        public static IEnumerable<SelectItem> EnumSelectListFor(this Type type)
        {
            if (type.IsEnum)
            {
                return BuildEnumSelectListItem(type);
            }
            else if (Nullable.GetUnderlyingType(type).IsEnum)
            {
                var nullableType = Nullable.GetUnderlyingType(type);
                return BuildEnumSelectListItem(nullableType);
            }

            return null;
        }

        private static IEnumerable<SelectItem> BuildEnumSelectListItem(Type t)
        {
            return Enum.GetValues(t)
                       .Cast<Enum>()
                       .Select(e => new SelectItem { id = Convert.ToInt32(e).ToString(), text = e.GetDescription() });
        }

        public static string DisplayCaseUserOrOrganization(this OrganizationUser user, string text = "無")
        {
            if (user == null)
                return "無";
            return user.UnitType == UnitType.Customer ? user.UserName : user.NodeName ?? "";
        }
    }
}
