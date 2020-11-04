using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests
{
    public static class TypeExtensions
    {
        private static Type[] nativeTypes = new[]
        {
            typeof(String),
            typeof(Decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };

        public static List<Tuple<string, string, string>> GetUnequalProperties<T1, T2>(this T1 self, T2 to, params string[] ignore)
            where T1 : class
            where T2 : class
        {
            if (self != null && to != null)
            {
                var type1 = typeof(T1);
                var type2 = typeof(T2);
                var unequalProperties =
                    from pi in type1.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where !ignore.Contains(pi.Name)
                    let selfValue = JsonConvert.SerializeObject(type1.GetProperty(pi.Name).GetValue(self, null))
                    let toValue = JsonConvert.SerializeObject(type2.GetProperty(pi.Name).GetValue(to, null))
                    where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
                    select new Tuple<string, string, string>(pi.Name, selfValue, toValue);
                return unequalProperties.ToList();
            }
            return null;
        }

        public static bool IsSimpleType(
           this Type type)
        {
            return
               type.IsValueType ||
               type.IsPrimitive ||
               nativeTypes.Contains(type) ||
               (Convert.GetTypeCode(type) != TypeCode.Object);
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                       "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    }
}
