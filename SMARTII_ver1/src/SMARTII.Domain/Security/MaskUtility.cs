using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SMARTII.Domain.Security
{
    public static class MaskUtility
    {
        public static string MaskPersonalInformation(string oriString, string maskString, Boolean isDismiss = true)
        {
            string oStr = "";
            oriString = oriString.Trim();
            char[] oStrArry = oriString.ToCharArray();
            int[] oArray = new int[] { 0, oriString.Trim().Length - 1 };

            if (Regex.IsMatch(oriString.Trim(), @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$"))
            {
                oStr += MaskPersonalInformation(oriString.Split('@')[0], maskString) + "@";

                for (int i = 0; i < oriString.Split('@')[1].Split('.').Length; i++)
                {
                    string oStrL = oriString.Split('@')[1].Split('.')[i].ToString();
                    if (i == 0)
                        oStr += MaskPersonalInformation(oStrL, maskString, false);
                    else
                        oStr += "." + MaskPersonalInformation(oStrL, maskString, false);
                }
                return oStr;
            }
            else if (Regex.IsMatch(oriString.Trim(), "^(09([0-9]){8})$"))
            {
                oArray = new int[] { 0, 1, 2, 3, 8, 9 };
            }
            else if (Regex.IsMatch(oriString.Trim(), "^[a-zA-Z][0-9]{9}$"))
            {
                oArray = new int[] { 0, 1, 7, 8, 9 };
            }

            for (int i = 0; i < oStrArry.Length; i++)
            {
                if (isDismiss)
                    oStr += oArray.Contains(i) ? oStrArry[i].ToString() : maskString;
                else
                    oStr += oArray.Contains(i) ? maskString : oStrArry[i].ToString();
            }
            return oStr;
        }
    }
}