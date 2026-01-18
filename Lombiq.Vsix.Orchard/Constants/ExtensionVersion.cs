namespace Lombiq.Vsix.Orchard.Constants
{
    internal static class ExtensionVersion
    {
        // This needs to be named "Version" for vsix-version-stamp to work during GitHub Actions builds. Using an
        // unrealistically high version number to ensure it's always higher than any released version and thus we can
        // run it from source without it conflicting with the installed version. Same as in
        // source.extension.vsixmanifest.
        public const string Version = "99.0.0";
    }
}
