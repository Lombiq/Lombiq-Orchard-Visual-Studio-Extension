using Lombiq.Vsix.Orchard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public abstract class FieldNameFromDependencyGeneratorBase : IFieldNameFromDependencyGenerator
    {
        public abstract double Priority { get; }

        public abstract bool CanGenerate(string dependency);

        public abstract DependencyInjectionData Generate(string dependency, bool useShortName);

        protected virtual string GetStringWithUnderscore(string value) =>
            "_" + value;

        [SuppressMessage(
            "Globalization",
            "CA1308:Normalize strings to uppercase",
            Justification = "This method is not used for string normalization. Lowercase is required here.")]
        protected virtual string GetLowerInvariantString(string value) =>
            value.ToLowerInvariant();

        protected virtual string GetLowerInvariantStringWithUnderscore(string value) =>
            GetStringWithUnderscore(GetLowerInvariantString(value));

        protected virtual string GetShortName(string value)
        {
            var upperCasedLetters = GetUpperCasedLetters(value);

            return upperCasedLetters.Any() ?
                GetLowerInvariantString(new string(upperCasedLetters.ToArray())) :
                value[0].ToString();
        }

        protected virtual string GetShortNameWithUnderscore(string value)
        {
            var upperCasedLetters = GetUpperCasedLetters(value);

            return upperCasedLetters.Any() ?
                GetLowerInvariantStringWithUnderscore(new string(upperCasedLetters.ToArray())) :
                GetStringWithUnderscore(value[0].ToString());
        }

        protected virtual string RemoveFirstLetterIfInterface(string interfaceName) =>
            interfaceName.Length > 1 && interfaceName.StartsWith("I", StringComparison.InvariantCulture) && char.IsUpper(interfaceName[1]) ?
                interfaceName.Substring(1) :
                string.Copy(interfaceName);

        [uppressMessage(
            "Globalization",
            "CA1308:Normalize strings to uppercase",
            Justification = "This method is not used for string normalization. Lowercase is required here.")]
        protected virtual string GetCamelCased(string value)
        {
            if (value.Length == 1) return value.ToLowerInvariant();

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        private static IEnumerable<char> GetUpperCasedLetters(string value) =>
            value.Where(letter => char.IsUpper(letter));
    }
}
