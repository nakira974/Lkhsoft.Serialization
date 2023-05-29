using System.Activities.Presentation.View;
using System.ComponentModel;

namespace Lkhsoft.Serialization.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for XmlDeserializationDesigner.xaml
    /// </summary>
    public partial class XmlDeserializationDesigner
    {
        public XmlDeserializationDesigner()
        {
            InitializeComponent();
        }
        
        private void TypeSelection_PropertyChanged( object sender, PropertyChangedEventArgs propertyChangedEventArgs )
        {
            var typePresenter = (TypePresenter)sender;
            var name          = typePresenter.Label.Replace( " ", "" );
        
            ModelItem.Properties[name].SetValue(typePresenter.Type);
        }
    }
}
