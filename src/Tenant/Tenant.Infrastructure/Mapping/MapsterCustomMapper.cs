using Common.Mapping;
using IMapper = MapsterMapper.IMapper;
namespace Tenant.Infrastructure.Mapping;

public class MapsterCustomMapper : ICustomMapper
{
    private readonly IMapper _mapper;

    public MapsterCustomMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TDestination>(object source) where TDestination : class
    {
        return _mapper.Map<TDestination>(source);
    }

    public List<TDestination> Map<TDestination>(List<object> source) where TDestination : class
    {
        return _mapper.Map<List<TDestination>>(source);
    }
}