namespace Common.Mapping;

public interface ICustomMapper
{
    TDestinition Map<TDestinition>(object source) where TDestinition : class;
    List<TDestinition> Map<TDestinition>(List<object> source) where TDestinition : class;
}