using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using XmlGoamlLibrary;

namespace XmlGoamlLibrary
{
	public class XmlDownloader
	{
		private readonly string _xsdSchemaPath;
		private ReportData _reportData;

		public XmlDownloader(string xsdSchemaPath)
		{
			_xsdSchemaPath = xsdSchemaPath;
			_reportData = new ReportData(); // Initialize with default empty data
		}

		// Method to set ReportData programmatically
		public void SetReportData(ReportData reportData)
		{
			_reportData = reportData;
		}

		// Method to generate the XML from the ReportData object
		public XDocument GenerateXml()
		{
			return SerializeReportData(_reportData);
		}

		// Serialize the ReportData object to XML
		private XDocument SerializeReportData(ReportData reportData)
		{
			using (var writer = new StringWriter())
			{
				var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ReportData));
				serializer.Serialize(writer, reportData);
				return XDocument.Parse(writer.ToString());
			}
		}

		// Validate the generated XML against the XSD
		public void ValidateXml(XDocument xDocument)
		{
			var schemas = new XmlSchemaSet();
			schemas.Add("", _xsdSchemaPath);

			bool errors = false;
			xDocument.Validate(schemas, (o, e) =>
			{
				Console.WriteLine($"{e.Severity}: {e.Message}");
				errors = true;
			});

			if (errors)
			{
				throw new Exception("XML validation against XSD failed.");
			}
		}

		// Save the XML document to a file
		public static void SaveXml(XDocument xDocument, string outputPath)
		{
			xDocument.Save(outputPath);
		}
	}
}
