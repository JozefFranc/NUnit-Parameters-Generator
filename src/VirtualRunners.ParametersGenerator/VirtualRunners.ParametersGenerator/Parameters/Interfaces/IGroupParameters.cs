namespace VirtualRunners.ParametersGenerator.Parameters.Interfaces
{
    public interface IGroupParameters : IGroup
    {
        IParameter[] Parameters { get; }
    }
}
