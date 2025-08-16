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
                    // --- ������ �κ� ---
                    System.Windows.MessageBox.Show("WebView2�� �ʱ�ȭ�� �� �����ϴ�.");
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
                    string scriptToInject = await File.ReadAllTextAsync("Assets/Scripts/lens_interop.js");
                    string scriptToExecute = $"{scriptToInject}; uploadImageFromBase64('{base64ImageData}');";
                    await coreWebView.ExecuteScriptAsync(scriptToExecute);
                }
                else
                {
                    // --- ������ �κ� ---
                    System.Windows.MessageBox.Show("Google Lens �������� ���� �� �����߽��ϴ�.");
                }
            }
            catch (Exception ex)
            {
                // --- ������ �κ� ---
                System.Windows.MessageBox.Show($"������ �߻��߽��ϴ�: {ex.Message}");
            }
        }
    }
}