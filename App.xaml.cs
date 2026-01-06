using System.Text;
using System.Windows;
using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Networking;

namespace ReceiptPrinterEmulator
{
    public partial class App : Application
    {
        public static ReceiptPrinter? Printer = null;
        public static NetServer? Server = null;

        static App()
        {
            // Register code pages provider to support legacy encodings (e.g., PC437, PC850, PC858)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Printer = new ReceiptPrinter(PaperConfiguration.Default);

            Server = new NetServer(9100);
            _ = Server.Run();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Server?.Stop();
        }
    }
}