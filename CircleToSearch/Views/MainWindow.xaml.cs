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
                    // --- 수정된 부분 ---
                    System.Windows.MessageBox.Show("WebView2를 초기화할 수 없습니다.");
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
                    // --- 수정된 부분 ---
                    System.Windows.MessageBox.Show("Google Lens 페이지를 여는 데 실패했습니다.");
                }
            }
            catch (Exception ex)
            {
                // --- 수정된 부분 ---
                System.Windows.MessageBox.Show($"오류가 발생했습니다: {ex.Message}");
            }
        }
    }
}