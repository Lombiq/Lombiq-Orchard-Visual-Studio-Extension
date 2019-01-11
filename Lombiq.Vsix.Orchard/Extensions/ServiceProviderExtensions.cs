using System.Collections.Generic;

namespace System
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class =>
            serviceProvider.GetService(typeof(T)) as T;

        public static IEnumerable<T> GetServices<T>(this IServiceProvider serviceProvider) where T : class =>
            serviceProvider.GetService(typeof(T)) as IEnumerable<T>;
    }
}
