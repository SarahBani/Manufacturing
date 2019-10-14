using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Core.DomainService
{
    public static class Extension
    {

        public static bool IsAssignedGenericType(this Type type, Type genericType) 
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
            Type baseType = type.BaseType;
            if (baseType == null)
            {
                return false;
            }
            return baseType.IsAssignedGenericType(genericType);
        }

        public static Type GetBaseDeclaringType(this MethodBase method)
        {
            Type declaringType = method.DeclaringType;
            while (declaringType.DeclaringType != null)
            {
                declaringType = declaringType.DeclaringType;
            }
            return declaringType;
        }

        public static MethodBase GetRealMethod(this MethodBase method)
        {
            Type generatedType = method.DeclaringType;
            Type originalType = generatedType.DeclaringType;
            if (originalType != null)
            {
                var matchingMethods =
                    from methodInfo in originalType.GetMethods()
                    let attr = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>()
                    where attr != null && attr.StateMachineType == generatedType
                    select methodInfo;

                if (matchingMethods.Any())
                {
                    return matchingMethods.Single();
                }
                else
                {
                    return null;
                }
            }
            return method;
        }

    }
}
