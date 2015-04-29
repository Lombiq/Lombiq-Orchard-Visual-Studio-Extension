using System;

namespace Lombiq.VisualStudioExtensions
{
    static class GuidList
    {
        public const string LombiqVisualStudioExtensionsPackageGuidString = "1a2f7a53-92bd-4396-b49c-98a9bfcc1d41";
        public const string LombiqVisualStudioExtensionsCommandSetGuidString = "0fe301eb-0ad4-4fb4-bbd9-c2545e74dde5";

        public const string LombiqVisualStudioExtensionsOptionsPageGuidString = "3F58B1E1-5717-49EB-AD1D-ECDE298A4BA9";

        public static readonly Guid LombiqVisualStudioExtensionsCommandSetGuid = new Guid(LombiqVisualStudioExtensionsCommandSetGuidString);
    };
}