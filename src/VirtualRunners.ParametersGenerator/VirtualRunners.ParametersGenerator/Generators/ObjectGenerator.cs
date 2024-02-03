using System.Collections;
using System.Reflection;
using VirtualRunners.ParametersGenerator.Generators.Base;
using VirtualRunners.ParametersGenerator.Parameters.Interfaces;

namespace VirtualRunners.ParametersGenerator.Generators
{
    public class ObjectGenerator<TObject> : GeneratorBase, IEnumerable<TObject>, IEnumerator<TObject>
    {
        private static readonly Type typeOfTObject = typeof(TObject);

        public TObject Current => CreateTObject();

        object IEnumerator.Current => Current!;

        public ObjectGenerator(IGroupParameters group)
            : base(group)
        {
            if (_group.Parameters.GroupBy(g => g.Name).Any(a => a.Count() > 1))
                throw new ArgumentException("Paremeter names are not unique");

            TObjectCheck();
        }

        private void TObjectCheck()
        {
            var members = typeOfTObject.GetMembers().Where(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field);

            foreach (var parameter in _group.Parameters)
            {
                var member = members.FirstOrDefault(m => m.Name == parameter.Name) ?? throw new ArgumentException($"Property or field named {parameter.Name} not found in class {typeOfTObject.Name}");
                if (member.MemberType == MemberTypes.Property)
                {
                    var property = (PropertyInfo)member;
                    if (property.PropertyType != parameter.Type)
                        throw new ArgumentException($"Property named {parameter.Name} has different type than defined parameter");
                }
                else
                {
                    var field = (FieldInfo)member;
                    if (field.FieldType != parameter.Type)
                        throw new ArgumentException($"Field named {parameter.Name} has different type than defined parameter");
                }
            }
        }

        private TObject CreateTObject()
        {
            var obj = Activator.CreateInstance<TObject>();
            foreach (var parameter in _group.Parameters)
            {
                var field = typeOfTObject.GetField(parameter.Name);
                field?.SetValue(obj, parameter.Current);
                if (field != null)
                    continue;

                var property = typeOfTObject.GetProperty(parameter.Name);
                property?.SetValue(obj, parameter.Current);
            }

            return obj;
        }

        public IEnumerator GetEnumerator()
            => this;

        IEnumerator<TObject> IEnumerable<TObject>.GetEnumerator()
            => this;

        public bool MoveNext()
            => _group.MoveNext();

        public void Reset()
            => _group.Reset();

        public void Dispose()
            => GC.SuppressFinalize(this);

    }
}
