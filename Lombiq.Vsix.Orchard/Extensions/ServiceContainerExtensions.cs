using System.Collections.Generic;

namespace System.ComponentModel.Design
{
    public static class ServiceContainerExtensions
    {
        public static void AddService<T>(this IServiceContainer container, T implementation) 
            where T : class =>
            container.AddService(typeof(T), implementation);

        public static void AddServices<T>(this IServiceContainer container, params T[] implementations)
            where T : class =>
            container.AddService(typeof(T), implementations);
    }
}
