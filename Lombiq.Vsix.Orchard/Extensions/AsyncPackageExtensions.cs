using EnvDTE;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Microsoft.VisualStudio.Shell
{
    public static class AsyncPackageExtensions
    {
        public static void AddService<T>(this AsyncPackage package, Func<System.Threading.Tasks.Task<object>> resolver) =>
            package.AddService(typeof(T), (container, cancellationToken, serviceType) => resolver());

        public static void AddService<TService, TImplementation>(this AsyncPackage package) where TImplementation : new() =>
            package.AddService<TService>(() => System.Threading.Tasks.Task.FromResult((object)new TImplementation()));

        public static void AddService<T>(this AsyncPackage package, T instance) =>
            ((IServiceContainer)package).AddService(typeof(T), instance);

        public static async System.Threading.Tasks.Task<T> GetServiceAsync<T>(this AsyncPackage package) =>
            (T)(await package.GetServiceAsync(typeof(T)));

        public static async System.Threading.Tasks.Task<IEnumerable<T>> GetServicesAsync<T>(this AsyncPackage package) =>
            (IEnumerable<T>)(await package.GetServiceAsync(typeof(T)));

        public static DTE GetDte(this AsyncPackage package) =>
            Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as DTE;
    }
}
