using FSAPortfolio.Application.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests
{
    public class CompareUtil
    {
        static PropertyInfo[] ProjectIgnoreList = {
            typeof(ProjectEditViewModel).GetProperty(nameof(ProjectEditViewModel.timestamp)),
            typeof(ProjectEditViewModel).GetProperty(nameof(ProjectEditViewModel.max_time))
        };


        private string rootPath;
        PropertyInfo[] ignoreList;
        public CompareUtil(string rootPath, PropertyInfo[] ignoreList)
        {
            this.rootPath = rootPath;
            this.ignoreList = ignoreList;
        }

        public static void Compare(ProjectEditViewModel expected, ProjectEditViewModel actual)
        {
            CompareProperties(expected.project_id, expected, actual, ProjectIgnoreList);
        }

        public static void CompareProperties(string name, object expected, object actual, PropertyInfo[] ignoreList = null)
        {
            var comparer = new CompareUtil(name, ignoreList);
            comparer.AssertAreEqual(expected, actual, name);
        }

        public void AssertAreEqual(object expected, object actual, string path)
        {
            if (expected != null && actual != null)
            {
                Type expectedType = expected.GetType();
                var expectedProperties = expectedType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && !ignoreList.Contains(p));

                foreach (PropertyInfo expectedProperty in expectedProperties)
                {
                    object expectedValue;
                    object actualValue;

                    expectedValue = expectedProperty.GetValue(expected, null);
                    actualValue = expectedProperty.GetValue(actual, null);

                    if (CanRecurse(expectedProperty.PropertyType))
                    {
                        AssertPathEqual(expectedValue, actualValue, $"{path}.{expectedProperty.Name}");
                    }
                    // Check collections
                    else if (typeof(IEnumerable).IsAssignableFrom(expectedProperty.PropertyType))
                    {
                        // null check
                        if (expectedValue == null && actualValue != null || expectedValue != null && actualValue == null)
                        {
                            AssertPathEqual(expectedValue, actualValue, $"{path}.{expectedProperty.Name}");
                        }
                        else if (expectedValue != null && actualValue != null)
                        {
                            IEnumerable<object> expectedItems = ((IEnumerable)expectedValue).Cast<object>();
                            IEnumerable<object> actualItems = ((IEnumerable)actualValue).Cast<object>();
                            int expectedCount = expectedItems.Count();
                            int actualCount = actualItems.Count();

                            AssertPathEqual(expectedCount, actualCount, $"{path}.{expectedProperty.Name}.Count()");
                            for (int i = 0; i < expectedCount; i++)
                            {
                                object expectedItem = expectedItems.ElementAt(i);
                                object actualItem = actualItems.ElementAt(i);
                                Type expectedItemType;

                                expectedItemType = expectedItem.GetType();

                                if (CanRecurse(expectedItemType))
                                {
                                    AssertPathEqual(expectedValue, actualValue, $"{path}.{expectedProperty.Name}");
                                }
                                else 
                                {
                                    AssertAreEqual(expectedItem, actualItem, $"{path}.{expectedProperty.Name}");
                                }
                            }
                        }
                    }
                    else if (expectedProperty.PropertyType.IsClass)
                    {
                        AssertAreEqual(expectedProperty.GetValue(expected, null), expectedProperty.GetValue(actual, null), $"{path}.{expectedProperty.Name}");
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else
                AssertPathEqual(expected, actual, path);

        }

        internal static void Compare(string original_Config_Expected, string original_Config_Actual)
        {
            throw new NotImplementedException();
        }

        private static bool CanRecurse(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }

        private void AssertPathEqual(object expected, object actual, string path)
        {
            Assert.AreEqual(expected, actual, $"{rootPath} properties don't match: {path} is [{actual ?? "null"}]; expected [{expected ?? "null"}]");
        }

    }
}
