using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vistian.Flutter.Remoting
{
    public class InvocationTarget
    {
        private readonly object[] _paramValues;
        private bool _isTask;
        private bool _isObservable;
        private bool _isVoid;
        private readonly object _serviceInstance;

        private readonly MethodInfo _method;

        public static InvocationTarget Create(object @object, string methodName, List<Parameter> parameters)
        {
            var method = @object.GetType().GetTypeInfo().GetDeclaredMethod(methodName);

            if (method == null)
            {
                throw InvocationException.MissingMethod(@object, methodName);
            }

            var methodParameters = method.GetParameters();

            // now then, for each method parameter we see if we can get the appropriate value from the jsonParameters
            // and if so
            // add that to our list of value for the invocation...

            var paramValues = new List<object>();

            foreach (var methodParameter in methodParameters)
            {
                var name = methodParameter.Name;

                var parameter = parameters.FirstOrDefault(p => p.Name == name);

                if (parameter != null)
                {
                    // we have a match !
                    var paramValue = parameter.Value;

                    var paramString = paramValue.ToString();

                    try
                    {
                        // now deserialize using the type of the parameters
                        var value = JsonConvert.DeserializeObject(paramString, methodParameter.ParameterType);

                        paramValues.Add(value);
                    }
                    catch (Exception)
                    {
                        throw InvocationException.InvalidParameterValue(@object, methodName, name, paramString);
                    }
                }
                else
                {
                    // do we do anything here, i.e. we've not had a parameter value through
                    if (!methodParameter.HasDefaultValue)
                    {
                        // need to throw an exception ...
                        throw InvocationException.ParameterNotOptional(@object, methodName, methodParameter.Name);
                    }
                }
            }

            return new InvocationTarget(@object, method, paramValues.ToArray());
        }

        public InvocationTarget(Object @object,MethodInfo method,object[] parameters)
        {
            _method = method;
            _paramValues = parameters;
            _serviceInstance = @object;
        }

        public Task<object> Invoke()
        {
            if (IsTask)
            {
                return ExecuteTask();
            }
            else
            {
                var result = Execute();

                return Task.FromResult<object>(result);
            }
        }

        private object Execute()
        {
            var result = _method.Invoke(_serviceInstance, _paramValues);

            // what do we get for void returns ?
            return result;
        }

        private async Task<object> ExecuteTask()
        {
            var resultTask = _method.Invoke(_serviceInstance, _paramValues);


            var task = (Task)resultTask;

            await task.ConfigureAwait(false);

            // what do we get for void returns ?
            return task.GetType().GetProperty("Result")?.GetValue(task);
        }

        public bool IsTask => (ReturnType.IsGenericType && ReturnType.GetGenericTypeDefinition() == typeof(Task<>));

        public bool IsObservable => ReturnType.IsGenericType && ReturnType.GetGenericTypeDefinition() == typeof(IObservable<>);

        public bool IsVoid => ReturnType == typeof(void);

        public Type ReturnType { get => _method.ReturnType; }
    }

    public class InvocationException:Exception
    {
        public InvocationException(object target,string methodName,string additional):base($"{target.GetType().Name} : {methodName} {additional}.")
        {
        }

        public static InvocationException ParameterNotOptional(object target,string methodName,string parameterName)
        {
            return new InvocationException(target,methodName,$"Parameter {parameterName} needs a value, but none was provided");
        }

        public static InvocationException InvalidParameterValue(object target,string methodName,string parameterName,string providedValue)
        {
            return new InvocationException(target, methodName, $"Parameter {parameterName} invalid assignment of '${providedValue}'");
        }

        public static InvocationException MissingMethod(object target,string methodName)
        {
            return new InvocationException(target,methodName, " missing method");
        }
    }

}
