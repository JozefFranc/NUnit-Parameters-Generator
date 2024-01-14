using System.Collections;
using VirtualRunners.ParametersGenerator.Parameters.Interfaces;

namespace VirtualRunners.ParametersGenerator.Parameters
{
    public class RangeParameter<T> : IParameter where T : struct, IComparable<T>
    {
        private readonly T _minValue;
        private readonly T _maxValue;
        private readonly T _incrementValue;

        private bool _isReseted = true;
        private T _current;

        public string Name { get; init; }
        public T Current => _current;

        object IEnumerator.Current => Current!;

        public Type Type { get; init; }

        public bool Disable { get; set; }

        public RangeParameter(string parameterName, T minValue, T maxValue, T incrementValue = default)
        {
            Name = parameterName;

            Type = typeof(T);

            if(!Type.IsPrimitive && Type != typeof(decimal))
                throw new ArgumentException("Not allowed type. Alloved types are primitive types (except IntPtr and UIntPtr). Also decimal is allowed");

            if (minValue.CompareTo(maxValue) > 0)
                throw new ArgumentException("Minimal value is greater than maximal value");

            _minValue = minValue;
            _maxValue = maxValue;

            if (incrementValue.CompareTo(default) == 0)
                incrementValue = SetIncrement(incrementValue);

            if(incrementValue.CompareTo(default)<0)
                throw new ArgumentException("Increment value needs to be positive number");

            _incrementValue = incrementValue;
            Reset();
        }

        public bool MoveNext()
        {
            if(Disable)
                return false;

            if(!_isReseted)
                _current = AddIncrement();

            if (Type == typeof(bool) && !_isReseted && !Convert.ToBoolean(_current))
                return false;

            _isReseted = false;
            return _current.CompareTo(_maxValue) <= 0;
        }

        public void Reset()
        {
            _isReseted = true;
            _current = _minValue;
        }

        public void Dispose()
            => GC.SuppressFinalize(this);

        private T AddIncrement()
        {
            switch (_current)
            {
                case bool boolValue:
                    return (T)(object)(bool)(!boolValue);
                case byte byteValue:
                    return (T)(object)(byte)(byteValue + Convert.ToByte(_incrementValue));
                case sbyte sbyteValue:
                    return (T)(object)(sbyte)(sbyteValue + Convert.ToSByte(_incrementValue));
                case short shortValue:
                    return (T)(object)(short)(shortValue + Convert.ToInt16(_incrementValue));
                case ushort ushortValue:
                    return (T)(object)(ushort)(ushortValue + Convert.ToUInt16(_incrementValue));
                case int intValue:
                    return (T)(object)(int)(intValue + Convert.ToInt32(_incrementValue));
                case uint uintValue:
                    return (T)(object)(uint)(uintValue + Convert.ToUInt32(_incrementValue));
                case long longValue:
                    return (T)(object)(long)(longValue + Convert.ToInt64(_incrementValue));
                case ulong ulongValue:
                    return (T)(object)(ulong)(ulongValue + Convert.ToUInt64(_incrementValue));
                case float floatValue:
                    return (T)(object)(float)(floatValue + Convert.ToSingle(_incrementValue));
                case double doubleValue:
                    return (T)(object)(double)(doubleValue + Convert.ToDouble(_incrementValue));
                case decimal decimalValue:
                    return (T)(object)(decimal)(decimalValue + Convert.ToDecimal(_incrementValue));
                default:
                    throw new ArgumentException("Unknown type");
            }
        }

        private T SetIncrement(T increment)
        {
            switch (increment)
            {
                case bool:
                    return (T)(object)true;
                case byte:
                    return (T)(object)Convert.ToByte(1);
                case sbyte:
                    return (T)(object)Convert.ToSByte(1);
                case short:
                    return (T)(object)Convert.ToInt16(1);
                case ushort:
                    return (T)(object)Convert.ToUInt16(1);
                case int:
                    return (T)(object)Convert.ToInt32(1);
                case uint:
                    return (T)(object)Convert.ToUInt32(1);
                case long:
                    return (T)(object)Convert.ToInt64(1);
                case ulong:
                    return (T)(object)Convert.ToUInt64(1);
                case float:
                case double:
                case decimal:
                    throw new ArgumentException("Increment value needs to be defined for floating point types or decimal");
                default:
                    throw new ArgumentException("Unknown type");
            }
        }
    }
}
