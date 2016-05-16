using System;

namespace Lombiq.VisualStudioExtensions.Constants
{
    static class PackageGuids
    {
        public const string LombiqVisualStudioExtensionPackageGuidString = "1a2f7a53-92bd-4396-b49c-98a9bfcc1d41";
        public const string LombiqVisualStudioExtensionCommandSetGuidString = "0fe301eb-0ad4-4fb4-bbd9-c2545e74dde5";

        // These GUIDs are also defined in the LombiqVisualStudioExtensions.vsct with the same value.
        public static readonly Guid LombiqVisualStudioExtensionCommandSetGuid = new Guid(LombiqVisualStudioExtensionCommandSetGuidString);
        public static readonly Guid LombiqVisualStudioExtensionPackageGuid = new Guid(LombiqVisualStudioExtensionPackageGuidString);
    };
}