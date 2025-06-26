using Syncfusion.Maui.DataGrid;

namespace SfDataGridSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.dataGrid.SerializationController = new SerializationControllerExt(this.dataGrid, Resources);
            this.dataGrid1.SerializationController = new SerializationControllerExt(this.dataGrid1, Resources);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string localPath = Path.Combine(FileSystem.AppDataDirectory, "DataGrid.xml");
            using (var file = File.Create(localPath))
            {
                dataGrid.Serialize(file);
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            string localPath = Path.Combine(FileSystem.AppDataDirectory, "DataGrid.xml");

            using (var file = File.Open(localPath, FileMode.Open))
            {
                dataGrid1.Deserialize(file);
            }
        }
    }

    public class SerializationControllerExt : DataGridSerializationController
    {

        private readonly ResourceDictionary _pageResources;

        public SerializationControllerExt(SfDataGrid grid, ResourceDictionary pageResources)
            : base(grid)
        {
            _pageResources = pageResources;
        }

        protected override void RestoreColumnProperties(SerializableDataGridColumn serializableColumn, DataGridColumn column)
        {
            base.RestoreColumnProperties(serializableColumn, column);

            if (column is DataGridTemplateColumn templateColumn &&
                string.Equals(templateColumn.MappingName, "Name", StringComparison.Ordinal))
            {
                if (_pageResources.TryGetValue("cellTemplate", out var template) &&
                    template is DataTemplate dataTemplate)
                {
                    templateColumn.CellTemplate = dataTemplate;
                }
            }
        }
    }

}