using EnvDTE;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Shell
{
    public static class AsyncPackageExtensions
    {
        public static void AddService<T>(this AsyncPackage package, Func<Task<object>> resolver) =>
            package.AddService(typeof(T), (container, cancellationToken, serviceType) => resolver());

        public static void AddService<TService, TImplementation>(this AsyncPackage package)
            where TImplementation : new() =>
            package.AddService<TService>(() => System.Threading.Tasks.Task.FromResult((object)new TImplementation()));

        public static void AddService<T>(this AsyncPackage package, T instance) =>
            ((IServiceContainer)package).AddService(typeof(T), instance);

        /// <summary>
        /// Gets the DTE object describing the VS IDE instance. Can be called from a background thread too. Don't cache
        /// it across scopes.
        /// </summary>
        public static Task<DTE> GetDteAsync(this AsyncPackage package) => package.GetServiceAsync<DTE>();

        public static async Task<T> GetServiceAsync<T>(this AsyncPackage package) =>
            (T)await package.GetServiceAsync(typeof(T)).ConfigureAwait(true);

        public static async Task<IEnumerable<T>> GetServicesAsync<T>(this AsyncPackage package) =>
            (IEnumerable<T>)await package.GetServiceAsync(typeof(T)).ConfigureAwait(true);
    }
}
