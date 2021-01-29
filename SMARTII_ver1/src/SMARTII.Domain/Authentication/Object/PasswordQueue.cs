using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Cache;
using SMARTII.Resource.Tag;

namespace SMARTII.Domain.Authentication.Object
{
    public class PasswordQueue
    {
        private Queue<string> _PasswordQueue = new Queue<string>();

        public PasswordQueue() : this(new string[] { })
        {
        }

        public PasswordQueue(string[] array)
        {
            _PasswordQueue = new Queue<string>(array);
        }

        public void Insert(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new Exception(Common_lang.PASSWORD_EMPTY);

            if (_PasswordQueue.Count < SecurityCache.PasswordResetLimitCount)
            {
                _PasswordQueue.Enqueue(password);
            }
            else
            {
                _PasswordQueue.Dequeue();
                _PasswordQueue.Enqueue(password);
            }
        }

        public Boolean HasAny(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new Exception(Common_lang.PASSWORD_EMPTY);

            return _PasswordQueue.Any(x => x == password?.Trim());
        }

        public string[] ToArray()
        {
            return _PasswordQueue.ToArray();
        }
    }
}