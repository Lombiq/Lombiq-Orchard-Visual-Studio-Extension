using EnvDTE;
using EnvDTE80;

namespace Lombiq.VisualStudioExtensions.Models
{
    public class CreateProjectContext
    {
        public Solution Solution { get; set; }

        public SolutionFolder SolutionFolder { get; set; }

        public string TemplatePath { get; set; }

        public string ProjectName { get; set; }
    }
}
