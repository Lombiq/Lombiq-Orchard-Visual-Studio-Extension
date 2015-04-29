using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Lombiq.VisualStudioExtensions
{
    [Guid(GuidList.LombiqVisualStudioExtensionsOptionsPageGuidString)]
    public class LombiqVisualStudioExtensionsOptionPage : DialogPage
    {
        [Category("Module Scaffolding")]
        [Description("Folder path where the Orchard template is located.")]
        [DisplayName("Template path")]
        public string OrchardModuleTemlatePath { get; set; }

        [Category("Module Scaffolding")]
        [Description("Author of the module to set in the Module.txt by default.")]
        [DisplayName("Default author")]
        public string DefaultAuthor { get; set; }

        [Category("Module Scaffolding")]
        [Description("Website of the module to set in the Module.txt by default.")]
        [DisplayName("Default website")]
        public string DefaultWebsite { get; set; }
    }
}
