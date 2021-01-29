using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SMARTII.Domain.Authentication.Object
{
    public static class PasswordUtility
    {
        public static bool Validate(this string password)
        {
            // 比對規則
            var _symbols_pattern = new Regex(@"[\W_]+");
            var _lowerLetters_pattern = new Regex("[a-z]");
            var _upperLetters_pattern = new Regex("[A-Z]");
            var _numbers_pattern = new Regex(@"[0-9]");

            // 比對結果
            var _flags = new List<bool>() {
                _lowerLetters_pattern.IsMatch(password),
                _upperLetters_pattern.IsMatch(password),
                _numbers_pattern.IsMatch(password),
                _symbols_pattern.IsMatch(password)
            };

            // 統計 match 數量
            var _passedMatches = 0;

            // 計算符合數量
            _flags.ForEach(_flag =>
            {
                _passedMatches += _flag == true ? 1 : 0;
            });

            return _passedMatches >= 3 && password.Length >= 8;
        }
    }
}