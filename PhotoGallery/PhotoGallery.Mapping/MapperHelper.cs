using AutoMapper;
using System;
using System.Collections.Generic;

namespace PhotoGallery.Mapping
{
    public static class MapperHelper
    {
        private static IMapper _mapper;
        private static bool _isReady;

        public static void Initalize(List<TypeContainer> bindings)
        {
            if(!_isReady)
            {
                MapperConfiguration _config = new MapperConfiguration(cfg =>
                {
                    foreach (var pair in bindings)
                    {
                        cfg.CreateMap(pair.SourceType,pair.DestinationType);
                        cfg.CreateMap(pair.DestinationType, pair.SourceType);
                    }
                });
                _mapper = _config.CreateMapper();
                _isReady = true;
            }
        }

        public static TDest GetValue<TSource,TDest>(TSource sourceObj)
        {
            if (!_isReady)
                throw new Exception("MapperHelper is not ready");

            return _mapper.Map<TSource, TDest>(sourceObj);
        }
    }
}
