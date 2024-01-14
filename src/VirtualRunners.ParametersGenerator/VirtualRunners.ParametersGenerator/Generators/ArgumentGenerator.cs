using NUnit.Framework;
using System.Collections;
using VirtualRunners.ParametersGenerator.Generators.Base;
using VirtualRunners.ParametersGenerator.Parameters.Interfaces;

namespace VirtualRunners.ParametersGenerator.Generators
{
    public class ArgumentGenerator : GeneratorBase, IEnumerable, IEnumerator
    {
        public object Current => Activator.CreateInstance(typeof(TestCaseData), _group.Parameters.Select(c => c.Current).ToArray())!;

        public ArgumentGenerator(IGroupParameters group)
            : base(group)
        {
        }

        public IEnumerator GetEnumerator()
            => this;

        public bool MoveNext()
            => _group.MoveNext();

        public void Reset()
            => _group.Reset();
    }
}
