using CatalogModule.ViewModels;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace CatalogModule.Views
{
    /// <summary>
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class SaleAnalysisCatalog : UserControl
    {
        private DataGridColumn _currentSortColumn;
        private ListSortDirection _currentSortDirection;

        public SaleAnalysisCatalog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows current sort direction on initial load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _currentSortDirection = ListSortDirection.Ascending;
            _currentSortColumn = dataGrid.Columns[0];
            _currentSortColumn.SortDirection = _currentSortDirection;
            dataGrid.Items.SortDescriptions.Add(new SortDescription(_currentSortColumn.SortMemberPath, _currentSortDirection));
        }

        /// <summary>
        /// Sets the sort direction for the current sorted column since the sort direction
        /// is lost when the DataGrid's ItemsSource property is updated.
        /// </summary>
        /// <param name="sender">The parts data grid.</param>
        /// <param name="e">Ignored.</param>
        private void DataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (_currentSortColumn != null)
            {
                _currentSortColumn.SortDirection = _currentSortDirection;
            }
        }

        private void CustomSorting(object sender, DataGridSortingEventArgs e)
        {
            dataGrid.Items.SortDescriptions.Clear();
            var direction = e.Column.SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            var sortField = e.Column.SortMemberPath;
            var viewModel = (ExcelCatalogViewModel)this.DataContext;

            viewModel.ApplyCustomSort(sortField, direction);
            e.Column.SortDirection = direction;
            _currentSortDirection = direction;
            _currentSortColumn = e.Column;
            e.Handled = true;
        }
    }
}
