using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models.JsonConverters
{
    /// <summary>
    /// Allows us to get a default JObject inside a converter without going into an infinite recursion.
    /// It does this by disabling the converter on that thread while the JObject is built (asp.net shares converters between threads, hence use of <see cref="ThreadStaticAttribute"/>
    /// </summary>
    /// <typeparam name="T">The type of object to be serialised</typeparam>
    public abstract class NonReentrantJsonConverter<T> : JsonConverter<T>
    {
        [ThreadStatic]
        static bool disabled;

        // Disables the converter in a thread-safe manner.
        bool Disabled { get { return disabled; } set { disabled = value; } }

        public override bool CanWrite { get { return !Disabled; } }

        protected JToken FromObject(object value, JsonSerializer serializer)
        {
            using (new PushValue<bool>(true, () => Disabled, (canWrite) => Disabled = canWrite))
            {
                return JToken.FromObject(value, serializer);
            }
        }
    }

    internal struct PushValue<T> : IDisposable
    {
        Action<T> setValue;
        T oldValue;

        internal PushValue(T value, Func<T> getValue, Action<T> setValue)
        {
            if (getValue == null || setValue == null)
                throw new ArgumentNullException();
            this.setValue = setValue;
            this.oldValue = getValue();
            setValue(value);
        }

        #region IDisposable Members

        // By using a disposable struct we avoid the overhead of allocating and freeing an instance of a finalizable class.
        public void Dispose()
        {
            if (setValue != null)
                setValue(oldValue);
        }

        #endregion
    }

}