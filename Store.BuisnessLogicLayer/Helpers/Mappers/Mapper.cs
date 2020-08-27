using System;
using System.Linq;

namespace Store.BuisnessLogic.Helpers
{
    public class Mapper<TSource, TDestination>
    {
        public TDestination Map(TDestination destination, TSource source)
        {
            Type _destination = typeof(TDestination);
            var sourcePropsInfo = source.GetType().GetProperties();
            var destinationPropsInfo = _destination.GetProperties();

            foreach (var srcPropInfo in sourcePropsInfo)
            {
                var destInfo = destinationPropsInfo.FirstOrDefault(dip => dip.Name == srcPropInfo.Name);
                if (destInfo != null && destInfo.CanWrite)
                {
                    try
                    {
                        destInfo.SetValue(destination, srcPropInfo.GetValue(source));
                    }
                    catch { }
                }
            }

            return destination;
        }

        public TDestination Map(TSource source)
        {
            var destination = Activator.CreateInstance<TDestination>();
            Type _destination = typeof(TDestination);
            var sourcePropsInfo = source.GetType().GetProperties();
            var destinationPropsInfo = _destination.GetProperties();

            foreach (var srcPropInfo in sourcePropsInfo)
            {
                var destInfo = destinationPropsInfo.FirstOrDefault(dip => dip.Name == srcPropInfo.Name);
                if (destInfo != null && destInfo.CanWrite)
                {
                    try
                    {
                        destInfo.SetValue(destination, srcPropInfo.GetValue(source));
                    }
                    catch { }
                }
            }

            return destination;
        }
    }
}
