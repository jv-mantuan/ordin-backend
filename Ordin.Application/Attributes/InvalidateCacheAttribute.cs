using Ordin.Application.Enums;

namespace Ordin.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InvalidateCacheAttribute(params CacheKeys[] cacheKeys) : Attribute
    {
        public CacheKeys[] CacheKeys { get; set; } = cacheKeys;
    }
}