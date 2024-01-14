using VirtualRunners.ParametersGenerator.Parameters.Interfaces;

namespace VirtualRunners.ParametersGenerator.Parameters
{
    public class CartesianParameterGroup : IGroupParameters
    {
        protected readonly IGroup[] _parameters;

        public IParameter[] Parameters => _parameters.SelectMany(p => (p is IParameter parameter) ? new[] { parameter } : ((IGroupParameters)p).Parameters).ToArray();

        public object Current => throw new NotImplementedException();

        public CartesianParameterGroup(params IGroup[] parameters)
        {
            _parameters = parameters;
        }

        public bool MoveNext()
        {
            for (int i = _parameters.Length - 1; i >= 0; i--)
            {
                if (_parameters[i].MoveNext())
                    return true;

                _parameters[i].Reset();
                _parameters[i].MoveNext();
            }
            return false;
        }

        public void Reset()
        {
            foreach (var parameter in _parameters)
                parameter.Reset();
        }
    }
}
