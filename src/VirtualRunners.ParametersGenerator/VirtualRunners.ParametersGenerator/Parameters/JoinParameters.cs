using VirtualRunners.ParametersGenerator.Parameters.Interfaces;

namespace VirtualRunners.ParametersGenerator.Parameters
{
    public class JoinParameters : IParameter
    {
        private int index = 0;
        private readonly IParameter[] items;

        public string Name { get; init; }
        public object Current => index >= 0 && index < items.Length ? items[index].Current : default!;

        public Type Type { get; init; }

        public bool Disable { get; set; }

        public JoinParameters(string parameterName, params IParameter[] parameters)
        {
            Name = parameterName;
            this.items = parameters;

            if ((parameters == null) || (parameters.Length < 2))
                throw new ArgumentException("Minimal two parameters are expected for join");

            Type = parameters[0].Type;
            for (int i = 1; i < parameters.Length; i++)
            {
                if (parameters[i].Type != Type)
                    throw new ArgumentException("Parameters have different types");
            }

            items = parameters;
        }

        public bool MoveNext()
        {
            if(Disable)
                return false;

            while (index < items.Length)
            {
                if (items[index].MoveNext())
                    return true;

                index++;
            }

            return false;
        }

        public void Reset()
        {
            foreach (var item in items)
                item.Reset();

            index = 0;
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}
