using System.Xml.Linq;
using XmlGoaml;

namespace XmlGoamlTests
{
	public class XmlDownloaderTests
	{
		private readonly XmlDownloader _downloader;

		public XmlDownloaderTests()
		{
			var xsdPath = Path.Combine(AppContext.BaseDirectory, "XmlSchema.xsd");

			// Debug output to ensure the correct path
			Console.WriteLine($"Looking for XSD file at path: {xsdPath}");

			// Verify if the file exists
			if (!File.Exists(xsdPath))
			{
				throw new FileNotFoundException($"XSD file not found at path: {xsdPath}");
			}

			_downloader = new XmlDownloader(xsdPath);
		}

		[Fact]
		public void Test_InitializeFromJson_WithValidJson_ShouldMergeData()
		{
			// Arrange
			var reportData = new ReportData
			{
				RentityId = "1111",
				Location = new ReportData.LocationData
				{
					Address = "Street 1"
				},
				Indicators = new List<string> { "0001M", "1131V", "2003G" },
				Activity = new ActivityData
				{
					ReportParties = new List<ActivityData.ReportParty>
					{
						new ActivityData.ReportParty
						{
							Role = "0",
							Person = new ActivityData.ReportParty.PersonData
							{
								FirstName = "First",
								LastName = "Last",
								Identifications = new List<ActivityData.ReportParty.PersonData.IdentificationData>
								{
									new ActivityData.ReportParty.PersonData.IdentificationData
									{
										Number = "A12345678"
									}
								}
							}
						}
					}
				}
			};

			// Act
			_downloader.SetReportData(reportData);
			XDocument xmlDocument = _downloader.GenerateXml();

			// Assert
			Assert.NotNull(xmlDocument);
			Assert.Contains("1111", xmlDocument.ToString());  // Checking RentityId
			Assert.Contains("Street 1", xmlDocument.ToString());  // Checking Address
			Assert.Contains("0001M", xmlDocument.ToString());  // Checking Indicator
			Assert.Contains("A12345678", xmlDocument.ToString());  // Checking Identification Number
		}

		[Fact]
		public void Test_SetMultipleIndicators_ShouldRetainAllIndicators()
		{
			// Arrange
			var reportData = new ReportData
			{
				Indicators = new List<string> { "0001M", "1131V", "2003G", "0004X", "0005Y" }
			};

			// Act
			_downloader.SetReportData(reportData);
			XDocument xmlDocument = _downloader.GenerateXml();

			// Assert
			Assert.Contains("0001M", xmlDocument.ToString());
			Assert.Contains("1131V", xmlDocument.ToString());
			Assert.Contains("2003G", xmlDocument.ToString());
			Assert.Contains("0004X", xmlDocument.ToString());
			Assert.Contains("0005Y", xmlDocument.ToString());
		}

		[Fact]
		public void Test_SaveXml_ShouldSaveFile()
		{
			// Arrange
			var reportData = new ReportData
			{
				RentityId = "67890",
				Location = new ReportData.LocationData
				{
					Address = "456 Another St"
				}
			};

			_downloader.SetReportData(reportData);
			XDocument xmlDocument = _downloader.GenerateXml();
			string tempFilePath = Path.GetTempFileName();

			try
			{
				// Act
				XmlDownloader.SaveXml(xmlDocument, tempFilePath); // Use the class name to call the static method

				// Assert
				Assert.True(File.Exists(tempFilePath));
				string savedXml = File.ReadAllText(tempFilePath);
				Assert.Contains("67890", savedXml);  // This assumes the RentityId is correctly populated in the XML.
				Assert.Contains("456 Another St", savedXml);  // Check for the address
			}
			finally
			{
				// Cleanup
				if (File.Exists(tempFilePath))
				{
					File.Delete(tempFilePath);
				}
			}
		}
	}
}
