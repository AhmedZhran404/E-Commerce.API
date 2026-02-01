using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResposes
{
    public class Result
    {
        private readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count == 0; // True

        public bool IsFailure => !IsSuccess;

        public IReadOnlyList<Error> Errors => _errors;

        // Success

        protected Result() {}

        // Failure - Single Error

        protected Result(Error error)
        {
            _errors.Add(error);
        }

        // Failure - More Than Error

        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        public static Result Ok() => new Result();
        public static Result Fail(Error error) => new Result(error);
        public static Result Fail(List<Error> errors) => new Result(errors);
   
        
    
    }


    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public TValue Value => IsSuccess ? _value : 
            throw new InvalidOperationException("You Can Not Access The Value In Case Of The Failure Scenario");

        private Result(TValue value) : base()
        {
            _value = value;
        }

        private Result(Error error) : base(error) 
        {
            _value = default!;
        }

        private Result(List<Error> errors) : base(errors) 
        {
            _value = default!;
        }

        // public static Factory Method
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public new static  Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public new static Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);
        


        // Operator Overloading
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    
    }
}
