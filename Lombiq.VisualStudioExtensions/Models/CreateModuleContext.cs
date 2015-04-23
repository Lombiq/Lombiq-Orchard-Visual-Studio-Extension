namespace Lombiq.VisualStudioExtensions.Models
{
    public class CreateModuleContext : CreateProjectContext
    {
        public string Author { get; set; }

        public string Website { get; set; }

        public string Description { get; set; }
    }
}
