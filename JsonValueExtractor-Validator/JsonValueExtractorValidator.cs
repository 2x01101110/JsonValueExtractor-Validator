using JsonValueExtractor_Validator.Exceptions;
using JsonValueExtractor_Validator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace JsonValueExtractor_Validator
{
    public class JsonValueExtractorValidator : IJsonValueExtractorValidator
    {
        private static List<KeyValuePair<string, object>> keyValues;
        private static JsonElement jsonElement;

        public IEnumerable<KeyValuePair<string, object>> Extract(string json)
        {
            keyValues = new List<KeyValuePair<string, object>>();

            if (string.IsNullOrEmpty(json)) throw new InvalidJsonException("Provided json string is null or empty.");

            try
            {
                jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidJsonException("Provided json string is not valid.", ex);
            }

            GetKeyValues(new KeyValuePair<string, JsonElement>(null, jsonElement));
            return keyValues;
        }
        public bool Validate(IEnumerable<KeyValuePair<string, object>> keyValues, IEnumerable<Condition> conditions)
        {
            bool allKeyValuesValid = true;

            foreach (Condition condition in conditions)
            {
                var keyValue = keyValues
                    .FirstOrDefault(kv => 
                        kv.Value.GetType() == condition.Value.GetType() &&
                        (kv.Key?.ToLowerInvariant() ?? "") == (condition.Key?.ToLowerInvariant() ?? "")
                     );

                if (keyValue.Equals(default(KeyValuePair<string, JsonElement>)))
                {
                    allKeyValuesValid = false;
                    break;
                }

                allKeyValuesValid = Validate(condition.Value, condition.Operator, keyValue.Value);

                if (!allKeyValuesValid) break;
            }

            return allKeyValuesValid;
        }
        public bool Validate<T>(T conditionValue, Operator @operator, T jsonValue)
        {
            if (conditionValue.GetType() == typeof(string) && @operator != Operator.Equals) return false;

            switch (@operator)
            {
                case Operator.Equals:
                    return EqualityComparer<T>.Default.Equals(conditionValue, jsonValue);
                case Operator.GreaterThan:
                    return Comparer<T>.Default.Compare(conditionValue, jsonValue) > 0;
                case Operator.GreaterOrEqualTo:
                    return Comparer<T>.Default.Compare(conditionValue, jsonValue) >= 0;
                case Operator.LessThan:
                    return Comparer<T>.Default.Compare(conditionValue, jsonValue) < 0;
                case Operator.LessThanOrEqualTo:
                    var test = Comparer<T>.Default.Compare(conditionValue, jsonValue) <= 0;
                    return test;
            }
            return false;
        }
        public bool ExtractAndValidate(string json, IEnumerable<Condition> conditions)
        {
            var keyValues = Extract(json);
            var keyValuesAreValid = Validate(keyValues, conditions);
            return keyValuesAreValid;
        }

        private static void GetKeyValues(KeyValuePair<string, JsonElement> keyValue)
        {
            switch (keyValue.Value.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty item in keyValue.Value.EnumerateObject())
                        GetKeyValues(new KeyValuePair<string, JsonElement>(item.Name, item.Value));
                    break;
                case JsonValueKind.Array:
                    foreach (JsonElement item in keyValue.Value.EnumerateArray())
                        GetKeyValues(new KeyValuePair<string, JsonElement>(null, item));
                    break;
                case JsonValueKind.String:
                    keyValues.Add(new KeyValuePair<string, object>(keyValue.Key, keyValue.Value.GetString()));
                    break;
                case JsonValueKind.Number:
                    keyValues.Add(new KeyValuePair<string, object>(keyValue.Key, keyValue.Value.GetDouble()));
                    break;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    if (!string.IsNullOrEmpty(keyValue.Key))
                        keyValues.Add(new KeyValuePair<string, object>(keyValue.Key, keyValue.Value.GetBoolean()));
                    break;
                case JsonValueKind.Null:
                    if (!string.IsNullOrEmpty(keyValue.Key))
                        keyValues.Add(new KeyValuePair<string, object>(keyValue.Key, null));
                    break;
                default:
                    break;
            }
        }
    }
}
