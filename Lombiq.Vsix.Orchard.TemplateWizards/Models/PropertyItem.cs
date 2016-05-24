using System.ComponentModel;

namespace Lombiq.Vsix.Orchard.TemplateWizards.Models
{
    public class PropertyItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
        [DisplayName("Hybrid Infoset")]
        public bool HybridInfoset { get; set; }
        [DisplayName("Skip from Template")]
        public bool SkipFromShapeTemplate { get; set; }


        public PropertyItem()
        {
            Type = "string";
        }
    }
}
