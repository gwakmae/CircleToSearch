// File: C:\Users\Public\Documents\C#_Code\CircleToSearch\CircleToSearch\Views\MainWindow.xaml.cs

using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace CircleToSearch.Wpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MainWindow_Loaded;
            InitializeWebViewAsync();
        }

        private async void InitializeWebViewAsync()
        {
            await WebView.EnsureCoreWebView2Async(null);
        }

        public async Task SearchWithImage(string base64ImageData)
        {
            try
            {
                await WebView.EnsureCoreWebView2Async(null);

                var coreWebView = WebView.CoreWebView2;
                if (coreWebView is null)
                {
                    System.Windows.MessageBox.Show("Failed to initialize WebView2.");
                    return;
                }

                var navigationTcs = new TaskCompletionSource<bool>();

                EventHandler<CoreWebView2NavigationCompletedEventArgs>? navigationCompletedHandler = null;
                navigationCompletedHandler = (sender, args) =>
                {
                    coreWebView.NavigationCompleted -= navigationCompletedHandler;
                    navigationTcs.TrySetResult(args.IsSuccess);
                };

                coreWebView.NavigationCompleted += navigationCompletedHandler;
                coreWebView.Navigate("https://lens.google.com/");

                bool navigationSucceeded = await navigationTcs.Task;

                if (navigationSucceeded)
                {
                    // script 경로를 실행 디렉터리 기준으로 안전하게 조합
                    string scriptPath = System.IO.Path.Combine(
                        AppContext.BaseDirectory, "Assets", "Scripts", "lens_interop.js");

                    string scriptToInject = await File.ReadAllTextAsync(scriptPath);
                    string scriptToExecute = $"{scriptToInject}; uploadImageFromBase64('{base64ImageData}');";
                    await coreWebView.ExecuteScriptAsync(scriptToExecute);
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed to open the Google Lens page.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
