# How-to-Serialize-template-column-content-in-.NET-MAUI-DataGrid--SfDataGrid-

This sample shows how to serialize and deserialize template column content in the .NET MAUI DataGrid (SfDataGrid).

The example demonstrates placing a `DataGridTemplateColumn` with a `CellTemplate` (defined in XAML resources), wiring a custom `DataGridSerializationController` to preserve template column configuration, and using simple Serialize/Deserialize buttons to write/read the grid state to an XML file in app data.

**Refer to the official UG for more details:**

- [Getting Started with .NET MAUI DataGrid](https://help.syncfusion.com/maui/datagrid/getting-started)
- [Serialization and Deserialization in .NET MAUI DataGrid (SfDataGrid)](https://help.syncfusion.com/maui/datagrid/serialization)

## XAML 

The grid includes a `DataGridTemplateColumn` whose `CellTemplate` is stored in page resources with x:Key="cellTemplate".

```xml
<ContentPage.BindingContext>
    <local:EmployeeViewModel x:Name="viewModel" />
</ContentPage.BindingContext>

<ContentPage.Resources>
    <ResourceDictionary>
        <DataTemplate x:Key="cellTemplate">
            <Button Text="Test" />
        </DataTemplate>
    </ResourceDictionary>
</ContentPage.Resources>

<Grid ColumnDefinitions="*,300"
        RowDefinitions="Auto,Auto">
    <syncfusion:SfDataGrid x:Name="dataGrid"
                            Grid.Column="0"
                            Grid.Row="0"
                            AllowGroupExpandCollapse="True"
                            ColumnWidthMode="Auto"
                            GridLinesVisibility="Both"
                            HeaderGridLinesVisibility="Both"
                            AutoGenerateColumnsMode="None"
                            ItemsSource="{Binding Employees}">

        <syncfusion:SfDataGrid.Columns>
            <syncfusion:DataGridNumericColumn MappingName="EmployeeID"
                                                HeaderText="Employee ID" />
            <syncfusion:DataGridTextColumn MappingName="Title"
                                            HeaderText="Designation" />
            <syncfusion:DataGridDateColumn MappingName="HireDate"
                                            HeaderText="Hire Date" />
            <syncfusion:DataGridTemplateColumn MappingName="Name"
                                                HeaderText="Employee Name"
                                                CellTemplate="{StaticResource cellTemplate}" />

        </syncfusion:SfDataGrid.Columns>
    </syncfusion:SfDataGrid>

    <syncfusion:SfDataGrid x:Name="dataGrid1"
                            Grid.Column="0"
                            Grid.Row="1"
                            ItemsSource="{Binding Employees}">
    </syncfusion:SfDataGrid>

    <VerticalStackLayout Grid.Column="1">
        <Grid RowDefinitions="80,80">
            <Button Text="Serialize"
                    Grid.Row="0"
                    Margin="0,0,0,20"
                    WidthRequest="150"
                    Clicked="Button_Clicked" />
            <Button Text="Deserialize"
                    Grid.Row="1"
                    Margin="0,20,0,0"
                    WidthRequest="150"
                    Clicked="Button_Clicked_1" />
        </Grid>
    </VerticalStackLayout>
</Grid>
```

## C# 

To support serialization and restore the `CellTemplate` after deserialization, the sample adds a small extension of `DataGridSerializationController` that looks up the template in the page `ResourceDictionary` and reassigns it when restoring column properties.


```csharp
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
```

## How it works
- The grid's `SerializationController` is set to `SerializationControllerExt` which inherits from `DataGridSerializationController`.
- When the grid is serialized, column definitions including type and properties are written to the stream (here, an XML file in `FileSystem.AppDataDirectory`).
- On deserialization the base controller restores basic column properties; `SerializationControllerExt.RestoreColumnProperties` runs afterward to reconnect runtime-only artifacts such as `DataTemplate` instances that live in the page `ResourceDictionary`.

##### Conclusion
 
I hope you enjoyed learning about how to Serialize template column content in .NET MAUI DataGrid (SfDataGrid).
 
You can refer to our [.NET MAUI DataGrid’s feature tour](https://www.syncfusion.com/maui-controls/maui-datagrid) page to learn about its other groundbreaking feature representations. You can also explore our [.NET MAUI DataGrid Documentation](https://help.syncfusion.com/maui/datagrid/getting-started) to understand how to present and manipulate data. 
For current customers, you can check out our .NET MAUI components on the [License and Downloads](https://www.syncfusion.com/sales/teamlicense) page. If you are new to Syncfusion, you can try our 30-day [free trial](https://www.syncfusion.com/downloads/maui) to explore our .NET MAUI DataGrid and other .NET MAUI components.
 
If you have any queries or require clarifications, please let us know in the comments below. You can also contact us through our [support forums](https://www.syncfusion.com/forums), [Direct-Trac](https://support.syncfusion.com/create) or [feedback portal](https://www.syncfusion.com/feedback/maui?control=sfdatagrid), or the feedback portal. We are always happy to assist you!
