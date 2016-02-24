using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FunctionalTester.InterpComponents
{
    class InterpValue
    {
        public ValueType Type { get; private set; }
        
        public int IntValue { get; private set; }
        public bool BoolValue { get; private set; }
        public string StringValue { get; private set; }
        public ProcessWrapper ProcessValue { get; private set; }

        public InterpValue(int value)
        {
            Type = ValueType.Integer;
            IntValue = value;
        }

        public InterpValue(bool value)
        {
            Type = ValueType.Boolean;
            BoolValue = value;
        }

        public InterpValue(string value)
        {
            Type = ValueType.String;
            StringValue = value;
        }

        public InterpValue(Process value)
        {
            Type = ValueType.Process;
            ProcessValue = new ProcessWrapper(value);
        }

        public InterpValue()
        {
            Type = ValueType.Null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var bVal = obj as InterpValue;
            if (bVal == null)
                return false;

            if (Type != bVal.Type)
                return false;

            switch (Type)
            {
                case ValueType.Null:
                    return true;
                case ValueType.Integer:
                    return IntValue == bVal.IntValue;
                case ValueType.Boolean:
                    return BoolValue == bVal.BoolValue;
                case ValueType.Process:
                    return ProcessValue == bVal.ProcessValue;
                case ValueType.String:
                    return StringValue == bVal.StringValue;
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string val = string.Empty;
            switch (Type)
            {
                case ValueType.Boolean:
                    val = BoolValue.ToString();
                    break;
                case ValueType.Integer:
                    val = IntValue.ToString();
                    break;
                case ValueType.Null:
                    break;
                case ValueType.Process:
                    val = ProcessValue.ToString();
                    break;
                case ValueType.String:
                    val = '"' + Regex.Escape(StringValue) + '"';
                    break;
            }

            return $"[{Type}] {val}";
        }
    }
}
