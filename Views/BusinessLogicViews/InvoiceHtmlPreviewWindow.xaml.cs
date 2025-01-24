using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PDAB.Views;

public partial class InvoiceHtmlPreviewWindow : Window
{
    public InvoiceHtmlPreviewWindow(string html)
    {
        InitializeComponent();
        var tempPath = Path.GetTempFileName() + ".html";
        File.WriteAllText(tempPath, html);
        HtmlPreview.Navigate(tempPath);
        
        Closed += (s, e) => 
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        };
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}