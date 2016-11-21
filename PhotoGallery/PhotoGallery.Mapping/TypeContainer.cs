using System;

namespace PhotoGallery.Mapping
{
    public class TypeContainer
    {
        public Type SourceType { get; set; }

        public Type DestinationType { get; set; }

        public TypeContainer(Type sourceType,Type destType)
        {
            SourceType = sourceType;
            DestinationType = destType;
        }
    }
}
