using System;
using System.Linq;

namespace Store.BuisnessLogic.Helpers
{
    public class Mapper<TSource, TDestination>
    {
        public TDestination Map(TDestination destination, TSource source)
        {
            var typeOfDestination = typeof(TDestination);
            var sourcePropsInfo = source.GetType().GetProperties();
            var destinationPropsInfo = typeOfDestination.GetProperties();

            foreach (var srcPropInfo in sourcePropsInfo)
            {
                var destPropertyInfo = destinationPropsInfo.FirstOrDefault(dip => dip.Name == srcPropInfo.Name);
                if (destPropertyInfo == null || !destPropertyInfo.CanWrite || destPropertyInfo.PropertyType != srcPropInfo.PropertyType)
                {
                    continue;
                }
                destPropertyInfo.SetValue(destination, srcPropInfo.GetValue(source));
            }
            return destination;
        }

        public TDestination Map(TSource source)
        {
            if (source == null)
            {
                return Activator.CreateInstance<TDestination>();
            }
            var destination = Activator.CreateInstance<TDestination>();
            var typeOfDestination = typeof(TDestination);
            var sourcePropsInfo = source.GetType().GetProperties();
            var destinationPropsInfo = typeOfDestination.GetProperties();

            foreach (var srcPropInfo in sourcePropsInfo)
            {
                var destPropertyInfo = destinationPropsInfo.FirstOrDefault(dip => dip.Name == srcPropInfo.Name);
                if (destPropertyInfo == null || !destPropertyInfo.CanWrite || destPropertyInfo.PropertyType != srcPropInfo.PropertyType)
                {
                    continue;
                }
                destPropertyInfo.SetValue(destination, srcPropInfo.GetValue(source));
            }

            return destination;
        }
    }
}
