using System;

namespace UtilityExtensions.Core.Validations
{
    public class Validation<T>
    {
        public int Order { get; private set; }
        public string name;
        public Func<T, bool> execute;
        public string onFailError;

        public Validation(int order, Func<T, bool> function, string onFailError, string name = "")
        {
            this.Order = order;
            this.execute = function;
            this.onFailError = onFailError;
            this.name = name;
        }
    }
}