using System;
using Xunit;

namespace JsonValueExtractor_Validator.Test
{
    public class ValidateSingleKeyValue
    {
        private IJsonValueExtractorValidator jsonValueExtractorValidator;
        public ValidateSingleKeyValue()
        {
            jsonValueExtractorValidator = new JsonValueExtractorValidator();
        }

        [Fact]
        public void two_string_are_equal()
        {
            var isEqual = this.jsonValueExtractorValidator.Validate("last", Models.Operator.Equals, "last");
            Assert.True(isEqual);
        }

        [Fact]
        public void two_string_are_not_equal()
        {
            var isEqual = this.jsonValueExtractorValidator.Validate("last", Models.Operator.Equals, "last_x");
            Assert.False(isEqual);
        }

        [Fact]
        public void two_strings_are_equal_but_invalid_operator()
        {
            var isEqual = this.jsonValueExtractorValidator.Validate("last", Models.Operator.GreaterOrEqualTo, "last_x");
            Assert.False(isEqual);
        }

        [Fact]
        public void two_numeric_values_are_equal()
        {
            var isEqual = this.jsonValueExtractorValidator.Validate(2, Models.Operator.Equals, 2);
            Assert.True(isEqual);
        }

        [Fact]
        public void first_numeric_value_greater_than_second()
        {
            var isEqual = this.jsonValueExtractorValidator.Validate(3.3434, Models.Operator.GreaterThan, 2);
            Assert.True(isEqual);
        }
    }
}
