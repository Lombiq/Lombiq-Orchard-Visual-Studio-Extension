using System;

namespace Lombiq.Vsix.Orchard.Constants
{
    internal static class PackageGuids
    {
        public const string LombiqOrchardVisualStudioExtensionPackageGuidString = "1a2f7a53-92bd-4396-b49c-98a9bfcc1d41";
        public const string LombiqOrchardVisualStudioExtensionCommandSetGuidString = "0fe301eb-0ad4-4fb4-bbd9-c2545e74dde5";

        // These GUIDs are also defined in the LombiqOrchardVisualStudioExtension.vsct with the same value.
        public static readonly Guid LombiqOrchardVisualStudioExtensionCommandSetGuid = new Guid(LombiqOrchardVisualStudioExtensionCommandSetGuidString);
        public static readonly Guid LombiqOrchardVisualStudioExtensionPackageGuid = new Guid(LombiqOrchardVisualStudioExtensionPackageGuidString);
    };
}