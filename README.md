# XmlGoaml

XmlGoaml is a .NET project designed for processing XML data with advanced validation against XSD schemas, specifically targeting reports related to financial intelligence and suspicious activity reporting, aligned with the goAML standards.

## goAML

The goAML application is a fully integrated software solution developed specifically for use by Financial Intelligence Units (FIU's) and is one of UNODC's strategic responses to financial crime, including money-laundering and terrorist financing. Financial Intelligence Units play a leading role in any anti-money laundering regime as they are generally responsible for receiving, processing and analyzing reports made by financial institutions or other entities according to the requirements of domestic anti-money laundering laws and regulations. Such reports and other information gathered by FIU's often provide the basis for investigations into money laundering, terrorist financing and other serious offences. goAML is specifically designed to meet the data collection, management, analytical, document management, workflow and statistical needs of any Finance Intelligence Unit. The goAML application was developed by the Information Technology Service (ITS) of UNODC in partnership with the UNODC Global Programme Against Money Laundering, Proceeds of Crime and the Financing of Terrorism (GPML).

## Features

- **XML Generation**: Generate XML files from JSON input.
- **XML Validation**: Validate XML against a provided XSD schema.

## Getting Started

### Prerequisites

- .NET 7.0 SDK

### Building the Project

Clone the repository and build the project using the following commands:

```bash
git clone https://github.com/core-laboratories/XmlGoaml.git
cd XmlGoaml
dotnet build XmlGoamlSolution.sln
```

### Running the Tests

Note: Due to limitations with the provided XSD schema, the validation tests are excluded. The XML schema may include unsupported elements such as `assert` that are not compatible with the .NET XSD validation library. Consequently, these validation tests are not automated and require manual inspection or a different validation approach.

Run the tests using the following command:

```bash
dotnet test XmlGoamlSolution.sln
```

## Application Overview

The XmlGoaml application is designed to facilitate the creation, conversion, and validation of XML reports in compliance with the United Nations Office on Drugs and Crime's (UNODC) goAML standards. These standards are used globally for suspicious activity reporting by financial institutions to financial intelligence units.

For more information on goAML, you can visit the following resources:

- [goAML](https://www.unodc.org/unodc/en/money-laundering/goaml.html)
- [goAML Documentation](https://www.unodc.org/documents/money-laundering/goAML/GoAML_User_Manual.pdf)

## License

This project is licensed under the CORE License - see the [LICENSE](LICENSE) file for details.
