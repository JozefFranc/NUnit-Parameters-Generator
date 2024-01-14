namespace VirtualRunners.ParametersGenerator.Parameters.Interfaces
{
    public interface IParameter : IGroup
    {
        string Name { get; init; }
        Type Type { get; init; }
        bool Disable { get; set; }
    }

}
