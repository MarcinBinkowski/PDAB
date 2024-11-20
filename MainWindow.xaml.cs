using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB
{
    public partial class MainWindow : Window
    {
        private readonly PdabDbContext _context = new PdabDbContext();
        private readonly List<string> tablesWithoutForeignKeys = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            LoadTables();
        }

        private void LoadTables()
        {
            var tables = _context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct()
                .ToList();

            foreach (var table in tables)
            {
                if (!HasForeignKey(table))
                {
                    tablesWithoutForeignKeys.Add(table);
                }
            }

            TablesListBox.ItemsSource = tablesWithoutForeignKeys;
        }

        private bool HasForeignKey(string tableName)
        {
            var entityType = _context.Model.GetEntityTypes()
                .FirstOrDefault(t => t.GetTableName() == tableName);

            return entityType?.GetForeignKeys().Any() ?? false;
        }

        private void TablesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TablesListBox.SelectedItem != null)
            {
                string selectedTable = TablesListBox.SelectedItem.ToString();
                LoadTableData(selectedTable);
            }
        }

        private void LoadTableData(string tableName)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();
                var query = $"SELECT * FROM {tableName}";
                var adapter = new SqlDataAdapter(query, connection);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                DataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void EditRow_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button?.Tag as DataRowView;
            if (row != null)
            {
                // Implement your edit logic here
                MessageBox.Show($"Edit row with ID: {row["ID"]}");
            }
        }

        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button?.Tag as DataRowView;
            if (row != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete row with ID: {row["ID"]}?", "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                    {
                        connection.Open();
                        var query = $"DELETE FROM {TablesListBox.SelectedItem} WHERE ID = @ID";
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID", row["ID"]);
                            command.ExecuteNonQuery();
                        }
                    }
                    LoadTableData(TablesListBox.SelectedItem.ToString());
                }
            }
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            if (TablesListBox.SelectedItem != null)
            {
                string selectedTable = TablesListBox.SelectedItem.ToString();
                var newRowData = NewRowDataTextBox.Text;
                var values = newRowData.Split(',');

                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    connection.Open();
                    var query = $"INSERT INTO {selectedTable} VALUES ({string.Join(",", values.Select(v => $"'{v}'"))})";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                LoadTableData(selectedTable);
            }
        }
    }
}