using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Extensions.Logging
{
    public static partial class ILoggerExtensions
    {
        /// <summary>
        /// Begins a logical operation scope using reflection to add property values.
        /// This method should be used sparingly due to the performance implications.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="state">The state to decompose into property values.</param>
        /// <returns></returns>
        public static IDisposable Scope(this ILogger logger, object state)
        {
            Dictionary<string, object> scope = new Dictionary<string, object>();
            Type stateType = state.GetType();

            foreach (PropertyInfo propertyInfo in stateType.GetRuntimeProperties())
            {
                try
                {
                    scope.Add(propertyInfo.Name, propertyInfo.GetValue(state));
                }

                catch { }
            }

            return logger.BeginScope(scope);
        }
    }
}
