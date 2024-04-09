using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Shared
{
    public class Result
    {
        protected internal Result(bool isSuccess, Error error) 
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public Error Error { get;}
        public bool IsSuccess { get;}
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new(true, Error.None);
        public static Result Failure() => new(false, Error.NullVable);
        public static Result<T> Success<T>(T result) => new(result, true, Error.None);
        public static Result<T> Failure<T>(Error error) => new(default, false, error);
        public static Result<T> Create<T>(T result) => result is null ? Failure<T>(Error.NullVable) : Success(result);
    }

    public class Result<T> : Result
    {
        protected internal Result(T value, bool isSucces, Error error) : base(isSucces, error)
        {
            _value = value;
        }
        public T Value => IsSuccess? _value! : throw new InvalidOperationException("The value of a failure result can not be accessed.");
        private readonly T _value;
        public static implicit operator Result<T>(T result) => Create(result);
    }
}
