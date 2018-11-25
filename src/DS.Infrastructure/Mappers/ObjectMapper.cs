using System.Linq;

namespace DS.Infrastructure.Mappers
{
    public static class ObjectMapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination, params string[] except)
            where TSource : class
            where TDestination : class
        {
            var targetProperties = destination
                .GetType()
                .GetProperties()
                .Where(p => p.GetAccessors().All(a => a.IsPublic))
                .ToList();

            foreach (var propertyInfo in source.GetType().GetProperties().Where(p => p.GetAccessors().All(a => a.IsPublic)))
            {
                if (except.Contains(propertyInfo.Name))
                    continue;

                var targetProperty = targetProperties.FirstOrDefault(p => p.Name == propertyInfo.Name);
                if (targetProperty?.CanWrite ?? false)
                    targetProperty.SetValue(destination, propertyInfo.GetValue(source));
            }

            return destination;
        }
    }
}
