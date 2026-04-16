using Ordin.Application.Enums;

namespace Ordin.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CacheableQueryAttribute : Attribute
    {
        public required CacheKeys CacheKey { get; set; }
        public required int DurationInSeconds { get; set; }
    }
}