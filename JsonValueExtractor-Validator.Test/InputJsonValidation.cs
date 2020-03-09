using JsonValueExtractor_Validator.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JsonValueExtractor_Validator.Test
{
    public class InputJsonValidation
    {
        private IJsonValueExtractorValidator jsonValueExtractorValidator;
        public InputJsonValidation()
        {
            jsonValueExtractorValidator = new JsonValueExtractorValidator();
        }

        [Fact]
        public void input_json_is_valid()
        {
            var json = "{\"value\":234.23}";
            var keyValues = jsonValueExtractorValidator.Extract(json);
        }

        [Fact]
        public void input_json_is_not_valid()
        {
            var json = "{\"value\":234.23XA}";
            Assert.Throws<InvalidJsonException>(() => jsonValueExtractorValidator.Extract(json));
        }
    }
}
