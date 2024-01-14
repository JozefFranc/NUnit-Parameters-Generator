using VirtualRunners.ParametersGenerator.Parameters.Interfaces;

namespace VirtualRunners.ParametersGenerator.Generators.Base
{
    public abstract class GeneratorBase
    {
        protected readonly IGroupParameters _group;

        public GeneratorBase(IGroupParameters group)
        {
            _group = group;
        }
    }
}
