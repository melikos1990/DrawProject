using System;
using System.Collections.Generic;

namespace SMARTII.Domain.Data
{
    public class Select2Response
    {
        public List<SelectItem> items { get; set; } = new List<SelectItem>();

        public static List<SelectItem> ToSelectItems<T>(
            ICollection<T> value,
            Func<T, string> idSelector,
            Func<T, string> textSelector)
        {
            var items = new List<SelectItem>();

            foreach (var item in value)
            {
                string id = idSelector(item);
                string text = textSelector(item);

                items.Add(new SelectItem()
                {
                    id = id,
                    text = text
                });
            }

            return items;
        }
    }

    public class Select2Response<R>
    {
        public List<SelectItem<R>> items { get; set; } = new List<SelectItem<R>>();

        public static List<SelectItem<R>> ToSelectItems<T>(
            ICollection<T> value,
            Func<T, string> idSelector,
            Func<T, string> textSelector,
            Func<T, R> extendSelector)
        {
            var items = new List<SelectItem<R>>();

            foreach (var item in value)
            {
                string id = idSelector(item);
                string text = textSelector(item);
                R extendInfo = extendSelector(item);

                items.Add(new SelectItem<R>()
                {
                    id = id,
                    text = text,
                    extend = extendInfo
                });
            }

            return items;
        }
    }

    public class SelectItem
    {
        public SelectItem()
        {
        }

        public SelectItem(string id, string text)
        {
            this.id = id;
            this.text = text;
        }

        public string id { get; set; }
        public string text { get; set; }
    }

    public class SelectItem<T> : SelectItem
    {
        public T extend { get; set; }
    }
}
