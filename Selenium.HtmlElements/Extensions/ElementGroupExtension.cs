using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OpenQA.Selenium;

namespace HtmlElements.Extensions {

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ElementGroupAttribute : Attribute {

        public ElementGroupAttribute(params String[] groups) {
            Groups = groups;
        }

        public IEnumerable<String> Groups { get; private set; }

    }

    public static class ElementGroupExtension {

        public static IEnumerable<Tuple<IWebElement, String>> GetElementsByGroup(this Object @object, params String[] groups) {
            return @object.GetType().GetMembersFromGroups(groups)
                .Where(memberInfo => memberInfo.GetPropertyOrFieldType().IsWebElement())
                    .Select(memberInfo => Tuple.Create(memberInfo.GetPropertyOrFieldValue(@object) as IWebElement, memberInfo.Name));
        }
    }

}