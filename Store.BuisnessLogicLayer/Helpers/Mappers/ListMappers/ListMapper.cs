using System;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Helpers.Mappers.ListMappers
{
    public static class ListMapper<TDestination, TSource>
    {
        private static readonly Mapper<TSource, TDestination> _mapper;
        static ListMapper()
        {
            _mapper = new Mapper<TSource, TDestination>();
        }
        public static List<TDestination> Map(List<TSource> source)
        {
            var destiantion = Activator.CreateInstance<List<TDestination>>();
            if (source == null)
            {
                return destiantion;
            }
            foreach (var sourceItem in source)
            {
                var destinationItem = _mapper.Map(sourceItem);
                destiantion.Add(destinationItem);
            }
            return destiantion;
        }
    }
}
