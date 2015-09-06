using $rootnamespace$.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace $rootnamespace$.Drivers
{
    public class $contentpartname$PartDriver : ContentPartDriver<$contentpartname$Part>
    {
        protected override DriverResult Display($contentpartname$Part part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_$contentpartname$",
                () => shapeHelper.Parts_$contentpartname$());
        }

        protected override DriverResult Editor($contentpartname$Part part, dynamic shapeHelper)
        {
            return ContentShape("Parts_$contentpartname$_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.$contentpartname$",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor($contentpartname$Part part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }

        protected override void Exporting($contentpartname$Part part, ExportContentContext context)
        {
            ExportInfoset(part, context);
        }

        protected override void Importing($contentpartname$Part part, ImportContentContext context)
        {
            ImportInfoset(part, context);
        }
    }
}