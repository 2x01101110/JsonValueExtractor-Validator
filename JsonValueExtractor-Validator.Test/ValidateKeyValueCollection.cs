using JsonValueExtractor_Validator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace JsonValueExtractor_Validator.Test
{
    public class ValidateKeyValueCollection
    {
        private IJsonValueExtractorValidator jsonValueExtractorValidator;
        public ValidateKeyValueCollection()
        {
            jsonValueExtractorValidator = new JsonValueExtractorValidator();
        }

        [Fact]
        public void input_defined_json_keyvalues_conditions_are_valid()
        {
            var json = JsonSerializer.Serialize(new
            {
                a = "123",
                b = 23.123,
                c = new[] 
                { 
                    new { d = 1, c = new string [] { "test1", "test3" } },
                    new { d = 2, c = new string [] { } }
                }
            });

            var conditions = new List<Condition>
            {
                new Condition { Key = "a", Operator = Operator.Equals, Value = "123" },
                new Condition { Key = "d", Operator = Operator.GreaterOrEqualTo, Value = 0 },
                new Condition { Operator = Operator.Equals, Value = "test1" }
            };

            var keyvalues = jsonValueExtractorValidator.Extract(json);
            var validatedResult = jsonValueExtractorValidator.Validate(keyvalues, conditions);

            Assert.True(validatedResult);
        }

        [Fact]
        public void input_defined_json_keyvalues_conditions_are_not_valid()
        {
            var json = JsonSerializer.Serialize(new
            {
                a = "123",
                b = 23.123,
                c = new[]
                {
                    new { d = 1, c = new string [] { "test1", "test3" } },
                    new { d = 2, c = new string [] { } }
                }
            });

            var conditions = new List<Condition>
            {
                new Condition { Key = "a", Operator = Operator.Equals, Value = 123 },
                new Condition { Key = "d", Operator = Operator.GreaterOrEqualTo, Value = 0 },
                new Condition { Operator = Operator.Equals, Value = "test2" }
            };

            var keyvalues = jsonValueExtractorValidator.Extract(json);
            var validatedResult = jsonValueExtractorValidator.Validate(keyvalues, conditions);

            Assert.False(validatedResult);
        }
    }
}
