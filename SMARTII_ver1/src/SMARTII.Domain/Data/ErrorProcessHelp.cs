using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Data
{

    public class OutputException<T> : Exception
    {
        public OutputException(string mesg) : base(mesg) { }
        public OutputException(ErrorContext<T> context) : this(context.Message)
        {
            this.Errors = context.Elements;
            this.Context = context;
        }
        public List<T> Errors { get; }
        public ErrorContext<T> Context { get; }
    }

    public class ErrorContext<T>
    {
        public List<T> Elements { get; set; } = new List<T>();

        public bool InValid() => this.Elements != null && this.Elements.Count > 0;
        
        public int Count() => this.Elements?.Count() ?? 0;
        
        public void Add(T element) => this.Elements.Add(element);

        public void AddRange(List<T> elements) => this.Elements.AddRange(elements);
        
        public string Message { get; set; } = string.Empty;
    }


    public static class ErrorProcessHelp
    {
        public static void Invoker<T>(Action<ErrorContext<T>> action)
        {
            var context = new ErrorContext<T>();

            action(context);

            if (context.InValid())
            {
                throw new OutputException<T>(context);
            }
        }
    }
    
}
