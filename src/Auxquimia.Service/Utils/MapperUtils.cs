namespace Auxquimia.Utils
{
    using AutoMapper;
    using Auxquimia.Dto;

    /// <summary>
    /// Defines the <see cref="MapperUtils" />
    /// </summary>
    public static class MapperUtils
    {
        public static IMapper mapper;

        /// <summary>
        /// The PerformMapping
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <param name="source">The source<see cref="X"/></param>
        /// <returns>The <see cref="Y"/></returns>
        public static Y PerformMapping<X, Y>(this X source)
        {
            return mapper.Map<Y>(source);
        }

        /// <summary>
        /// The PerformMapping
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <param name="source">The source<see cref="X"/></param>
        /// <param name="target">The target<see cref="Y"/></param>
        /// <returns>The <see cref="Y"/></returns>
        public static Y PerformMapping<X, Y>(this X source, Y target)
        {
            return mapper.Map(source, target);
        }
    }

    public class MapperUtilsBootstrapper
    {
        public MapperUtilsBootstrapper(IMapper mapper)
        {
            MapperUtils.mapper = mapper;
        }
    }
}
