using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityExtensions.Core.Validations
{
    public abstract class ValidationManager
    {
        public struct Settings
        {
            public bool throwExceptionOnFail;
            public bool validateImmediately;
        }
    }

    public sealed class ValidationManager<T> : ValidationManager
    {
        public class Parameter
        {
            public T value;
            public string name;
            private Dictionary<int, Result> results = new Dictionary<int, Result>();
            public IReadOnlyCollection<Result> Results => results.Values.ToList().AsReadOnly();

            public void ClearResults()
            {
                results.Clear();
            }

            public Parameter(T value, string name)
            {
                this.value = value;
                this.name = name;
            }

            public void AddResult(int order, Result result)
            {
                this.results[order] = result;
            }
        }

        public struct Result
        {
            public enum State
            {
                Pending,
                Success,
                Failed,
                Exception
            }

            public bool IsSuccess => state == State.Success;
            public string paramName;
            public State state;
            public Exception exception;
            public Validation<T> validation;
        }

        public Settings settings { get; private set; }
        private List<Parameter> parameters = new List<Parameter>();
        private List<Validation<T>> validations = new List<Validation<T>>();

        public IReadOnlyCollection<Parameter> Parameters => parameters.AsReadOnly();
        public IReadOnlyCollection<Validation<T>> Validations => validations.AsReadOnly();

        private ValidationManager()
        {
        }

        private ValidationManager(Settings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Starts the validation chain by creating the ValidationManager object with settings
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public static ValidationManager<T> UseSettings(Settings settings)
        {
            return new ValidationManager<T>(settings);
        }

        /// <summary>
        /// Adds a value to be validated
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="value"> </param>
        /// <param name="paramName"> </param>
        /// <returns> </returns>
        public static ValidationManager<T> Add(T value, string paramName = "")
        {
            ValidationManager<T> v = new ValidationManager<T>(new Settings { throwExceptionOnFail = true, validateImmediately = true });

            v.AddParameter(value, paramName);

            return v;
        }

        public void AddParameter(T value, string paramName = "")
        {
            parameters.Add(new Parameter(value, paramName));
        }

        /// <summary>
        /// Returns a list with the parameters that failed any validations
        /// </summary>
        /// <returns> </returns>
        public IReadOnlyCollection<Parameter> FailedParameters()
        {
            List<Parameter> failedParameters = new List<Parameter>();

            foreach (Parameter param in parameters)
            {
                foreach (Result result in param.Results)
                {
                    if (!result.IsSuccess)
                    {
                        failedParameters.Add(param);
                        break;
                    }
                }
            }

            return failedParameters.AsReadOnly();
        }

        /// <summary>
        /// Adds a validation method
        /// </summary>
        /// <returns> </returns>
        /// <exception cref="ArgumentNullException"> </exception>
        /// <exception cref="ValidationException"> </exception>
        public void AddValidation(Func<T, bool> validationFunc, string validationError, string validationName = "")
        {
            if (validationFunc == null)
            {
                throw new ArgumentNullException(nameof(validationFunc));
            }

            Validation<T> validation = new Validation<T>(validations.Count, validationFunc, validationError, validationName);

            validations.Add(validation);

            if (settings.validateImmediately)
            {
                ExecuteValidation(validation);
            }
        }

        private void ExecuteValidation(Validation<T> validation)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                Parameter param = parameters[i];
                Result result = new Result { validation = validation, paramName = param.name };

                try
                {
                    result.state = validation.execute(param.value) ? Result.State.Success : Result.State.Failed;
                }
                catch (Exception e)
                {
                    result.state = Result.State.Exception;
                    result.exception = e;
                }

                if (!result.IsSuccess && settings.throwExceptionOnFail)
                {
                    ThrowValidationException(result);
                }

                param.AddResult(validation.Order, result);
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ValidationException"> </exception>
        public void Validate()
        {
            ClearResults();
            foreach (Validation<T> method in validations)
            {
                ExecuteValidation(method);
            }
        }

        public void Clear()
        {
            ClearParameters();
            ClearValidations();
        }

        public void ClearParameters()
        {
            parameters.Clear();
        }

        public void ClearValidations()
        {
            validations.Clear();
        }

        private void ClearResults()
        {
            if (parameters != null)
                foreach (var param in parameters)
                {
                    param.ClearResults();
                }
        }

        /// <summary>
        /// Throws a ValidationException
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="result"> </param>
        private void ThrowValidationException(Result result)
        {
            throw new ValidationException($"Parameter {result.paramName} failed validation '{result.validation.name ?? "Unnamed"}': {result.validation.onFailError}", result.paramName, result.validation.name, result.validation.onFailError, result.exception);
        }
    }
}