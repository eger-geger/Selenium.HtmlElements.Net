using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<Tuple<IWebElement, string>> GetElementsByGroup(this Object @object, params String[] groups) {
            return @object.GetType().GetMembersFromGroups(groups)
                .Where(memberInfo => MemberInfoExtensions.GetPropertyOrFieldType(memberInfo).IsWebElement())
                    .Select(memberInfo => Tuple.Create(memberInfo.ReadPropertyOrFieldValue(@object) as IWebElement, memberInfo.Name));
        }
    }

}