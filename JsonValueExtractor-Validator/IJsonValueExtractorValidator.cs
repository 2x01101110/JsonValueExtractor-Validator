using JsonValueExtractor_Validator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonValueExtractor_Validator
{
    public interface IJsonValueExtractorValidator
    {
        IEnumerable<KeyValuePair<string, object>> Extract(string json);
        bool Validate<T>(T conditionValue, Operator @operator, T jsonValue);
        bool Validate(IEnumerable<KeyValuePair<string, object>> keyValues, IEnumerable<Condition> conditions);
        bool ExtractAndValidate(string json, IEnumerable<Condition> conditions);
    }
}
