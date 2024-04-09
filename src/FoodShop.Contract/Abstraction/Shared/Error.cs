using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Shared
{
    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullVable = new("Error.NullValue", "The specified result value is null.");
        protected internal Error(string code, string message)
        {
            Message = message;
            Code = code;
        }
        public string Message { get; }
        public string Code { get; }

        public static implicit operator string(Error error) => error.Code;
        public override bool Equals(object? obj)
        {
            return obj is Error errorOther && this.Equals(errorOther);
        }
        public bool Equals(Error other) 
        {
            if (other == null)
            {
                return false;
            }
            return other.Message == Message && other.Code == Code;
        }
        public override int GetHashCode() => HashCode.Combine(Message, Code);   
    }
}
