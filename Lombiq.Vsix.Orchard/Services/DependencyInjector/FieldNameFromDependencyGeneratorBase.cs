using System.Collections.Generic;
using System.Linq;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public abstract class FieldNameFromDependencyGeneratorBase : IFieldNameFromDependencyGenerator
    {
        public abstract double Priority { get; }


        public abstract bool CanGenerate(string dependency);

        public abstract string Generate(string dependency, bool useShortName);


        protected virtual string GetStringWithUnderscore(string value) => 
            "_" + value;

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
            interfaceName.Length > 1 && interfaceName.StartsWith("I") && char.IsUpper(interfaceName[1]) ?
                interfaceName.Substring(1) :
                string.Copy(interfaceName);

        protected virtual string GetCamelCased(string value)
        {
            if (value.Length == 1) return value.ToLower();

            return char.ToLower(value[0]) + value.Substring(1);
        }


        private IEnumerable<char> GetUpperCasedLetters(string value) => 
            value.Where(letter => char.IsUpper(letter));
    }
}
