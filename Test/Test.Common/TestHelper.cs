using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Test.Common
{
    public static class TestHelper
    {

        public static TKey GetId<TKey>(int id)
        {
            return (TKey)Convert.ChangeType(id, typeof(TKey));
        }

        public static void AreEqualEntities<T>(T expected, T actual, string message)
        {
            Assert.IsTrue(ArePropertiesEqual(expected, actual), message);
        }

        public static void AreEqualEntities<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message)
        {
            bool isEqual = true;
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (!expected.Count().Equals(actual.Count()))
            {
                isEqual = false;
            }
            else
            {
                for (int i = 0; i < expected.Count(); i++)
                {
                    if (!ArePropertiesEqual(expected.ElementAt(i), actual.ElementAt(i), fields))
                    {
                        isEqual = false;
                    }
                }
            }
            Assert.IsTrue(isEqual, message);
        }

        private static bool ArePropertiesEqual<T>(T expected, T actual)
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            return ArePropertiesEqual(expected, actual, fields);
        }

        private static bool ArePropertiesEqual<T>(T expected, T actual, FieldInfo[] fields)
        {
            foreach (var field in fields)
            {
                var value1 = field.GetValue(expected);
                var value2 = field.GetValue(actual);
                if (value1 == null && value2 == null)
                {
                    continue;
                }
                if (!value1.Equals(value2))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
