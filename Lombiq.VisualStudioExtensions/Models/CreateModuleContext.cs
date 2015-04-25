using EnvDTE;
using EnvDTE80;

namespace Lombiq.VisualStudioExtensions.Models
{
    public class CreateModuleContext
    {
        public Solution Solution { get; set; }

        public SolutionFolder SolutionFolder { get; set; }

        public string TemplatePath { get; set; }

        public string ProjectName { get; set; }

        public string Author { get; set; }

        public string Website { get; set; }

        public string Description { get; set; }
    }
}
