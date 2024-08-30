using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using XmlGoamlLibrary;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia.Platform.Storage;

namespace XmlGoamlConsoleApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			// Define the XSD path
			string xsdPath = "XmlGoamlLibrary/XmlSchema.xsd";
			var downloader = new XmlDownloader(xsdPath);

			// Generate the XML document from the programmatically set data
			var xDocument = downloader.GenerateXml();

			// Validate the XML document against the XSD
			downloader.ValidateXml(xDocument);

			// Initialize Avalonia application to use the StorageProvider API
			var app = AppBuilder.Configure<App>()
								.UsePlatformDetect()
								.LogToTrace()
								.UseReactiveUI()
								.StartWithClassicDesktopLifetime(args);

			// Use the MainWindow or a Window instance to access the StorageProvider
			var mainWindow = new Window();

			// Use StorageProvider to save the file
			var storageProvider = mainWindow.StorageProvider;
			var filePickerResult = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
			{
				Title = "Save goAML Report",
				SuggestedFileName = "report.xml",
				FileTypeChoices = new List<FilePickerFileType>
				{
					new("XML files") { Patterns = new[] { "*.xml" } },
					new("All files") { Patterns = new[] { "*" } }
				},
				DefaultExtension = "xml"
			});

			if (filePickerResult != null)
			{
				// Save the validated XML document to the chosen location
				using var stream = await filePickerResult.OpenWriteAsync();
				using var writer = new StreamWriter(stream);
				xDocument.Save(writer);
				Console.WriteLine($"goAML saved successfully to {filePickerResult.Path}");
			}
			else
			{
				Console.WriteLine("Save operation was canceled.");
			}
		}

		private class App : Application { }
	}
}
