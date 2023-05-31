using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using Lkhsoft.Serialization.Activities.Design.Designers;
using Lkhsoft.Serialization.Activities.Design.Properties;

namespace Lkhsoft.Serialization.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(JsonDeserialization), categoryAttribute);
            builder.AddCustomAttributes(typeof(JsonDeserialization), new DesignerAttribute(typeof(JsonDeserializationDesigner)));
            builder.AddCustomAttributes(typeof(JsonDeserialization), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(JsonSerialization), categoryAttribute);
            builder.AddCustomAttributes(typeof(JsonSerialization), new DesignerAttribute(typeof(JsonSerializationDesigner)));
            builder.AddCustomAttributes(typeof(JsonSerialization), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(XmlDeserialization), categoryAttribute);
            builder.AddCustomAttributes(typeof(XmlDeserialization), new DesignerAttribute(typeof(XmlDeserializationDesigner)));
            builder.AddCustomAttributes(typeof(XmlDeserialization), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(XmlSerialization), categoryAttribute);
            builder.AddCustomAttributes(typeof(XmlSerialization), new DesignerAttribute(typeof(XmlSerializationDesigner)));
            builder.AddCustomAttributes(typeof(XmlSerialization), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
