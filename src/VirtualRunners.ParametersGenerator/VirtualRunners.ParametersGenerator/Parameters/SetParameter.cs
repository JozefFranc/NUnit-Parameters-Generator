using System.Collections;
using VirtualRunners.ParametersGenerator.Parameters.Interfaces;

namespace VirtualRunners.ParametersGenerator.Parameters
{
    public class SetParameter<T> : IParameter
    {
        private int index = -1;
        private readonly T[] items;

        public string Name { get; init; }
        public T Current => index >= 0 && index < items.Length ? items[index] : default!;

        object IEnumerator.Current => Current!;

        public Type Type { get; init; }

        public bool Disable { get; set; }

        public SetParameter(string parameterName, T[] items)
        {
            Name = parameterName;
            this.items = items;

            Type = typeof(T);
        }

        public bool MoveNext()
        {
            if(Disable)
                return false;

            index++;
            if (index < items.Length)
                return true;

            index = items.Length;
            return false;
        }

        public void Reset()
            => index = -1;

        public void Dispose()
            => GC.SuppressFinalize(this);

    }
}
