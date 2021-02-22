using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using QRCoder;

namespace QrCoder
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			SetTextFromClipboard();
		}


		private void SetTextFromClipboard()
		{
			if (!Clipboard.ContainsText())
				return;

			string text = Clipboard.GetText().Trim();

			if (text.Length == 0 || TextInput.Text == text)
				return;

			TextInput.Text = text;
			TextInput.CaretIndex = text.Length;
			TextInput.Focus();
		}

		private static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
		{
			MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

			BitmapImage image = new BitmapImage();
			image.BeginInit();
			image.StreamSource = new MemoryStream(ms.ToArray());
			image.EndInit();

			return image;
		}


		private void PasteButton_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			SetTextFromClipboard();
		}

		private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = TextInput.Text.Trim();

			if (string.IsNullOrEmpty(text))
			{
				MessagePanel.Visibility = Visibility.Visible;
				QrCodeOutput.Source = null;
			}
			else
			{
				MessagePanel.Visibility = Visibility.Collapsed;

				QRCodeGenerator qrGenerator = new QRCodeGenerator();
				QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
				QRCode qrCode = new QRCode(qrCodeData);
				System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(5, "#000000", "#A9A9A9");
				QrCodeOutput.Source = BitmapToBitmapImage(qrCodeImage);
			}
		}
	}
}