using System;
using System.Collections.Generic;
using System.Text;

namespace JsonValueExtractor_Validator.Models
{
    public class Condition
    {
        public string Key { get; set; }
        public Operator Operator { get; set; }
        public object Value { get; set; }
    }
}
