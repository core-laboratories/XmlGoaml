namespace XmlGoamlLibrary
{
	public class ReportData
	{
		public string? SchemaVersion { get; set; } = "5.0.2";
		public string? RentityId { get; set; }
		public string? SubmissionCode { get; set; } = "E";
		public string? ReportCode { get; set; }
		public string? EntityReference { get; set; }
		public DateTime ReportDate { get; set; } = DateTime.Now;
		public string? CurrencyCodeLocal { get; set; } = "CHF";
		public string? ReportingUserCode { get; set; }
		public LocationData? Location { get; set; } = new LocationData();
		public string? Reason { get; set; }
		public string? Action { get; set; }
		public ActivityData? Activity { get; set; } = new ActivityData();
		public List<string> Indicators { get; set; } = new List<string>();
		public AdditionalInformation? AdditionalInfo { get; set; } = new AdditionalInformation();

		public void Merge(ReportData? newData)
		{
			if (newData == null) return;

			// Merge simple properties
			SchemaVersion = newData.SchemaVersion ?? SchemaVersion;
			RentityId = newData.RentityId ?? RentityId;
			SubmissionCode = newData.SubmissionCode ?? SubmissionCode;
			ReportCode = newData.ReportCode ?? ReportCode;
			EntityReference = newData.EntityReference ?? EntityReference;
			ReportDate = newData.ReportDate != default(DateTime) ? newData.ReportDate : ReportDate;
			CurrencyCodeLocal = newData.CurrencyCodeLocal ?? CurrencyCodeLocal;
			ReportingUserCode = newData.ReportingUserCode ?? ReportingUserCode;
			Reason = newData.Reason ?? Reason;
			Action = newData.Action ?? Action;

			// Merge nested objects
			if (newData.Location != null)
			{
				Location = Location ?? new LocationData();
				Location.Merge(newData.Location);
			}

			if (newData.Activity != null)
			{
				Activity = Activity ?? new ActivityData();
				Activity.Merge(newData.Activity);
			}

			if (newData.Indicators != null && newData.Indicators.Count > 0)
			{
				Indicators = new List<string>(newData.Indicators);
			}

			if (newData.AdditionalInfo != null)
			{
				AdditionalInfo = AdditionalInfo ?? new AdditionalInformation();
				AdditionalInfo.Merge(newData.AdditionalInfo);
			}
		}

		public class LocationData
		{
			public string? AddressType { get; set; }
			public string? Address { get; set; }
			public string? City { get; set; }
			public string? Zip { get; set; }
			public string? CountryCode { get; set; }
			public string? State { get; set; }

			public void Merge(LocationData? newData)
			{
				if (newData == null) return;

				AddressType = newData.AddressType ?? AddressType;
				Address = newData.Address ?? Address;
				City = newData.City ?? City;
				Zip = newData.Zip ?? Zip;
				CountryCode = newData.CountryCode ?? CountryCode;
				State = newData.State ?? State;
			}
		}

		public class AdditionalInformation
		{
			public string? InfoType { get; set; }
			public int InfoNumeric { get; set; }

			public void Merge(AdditionalInformation? newData)
			{
				if (newData == null) return;

				InfoType = newData.InfoType ?? InfoType;
				InfoNumeric = newData.InfoNumeric != 0 ? newData.InfoNumeric : InfoNumeric;
			}
		}
	}

	public class ActivityData
	{
		public List<ReportParty> ReportParties { get; set; } = new List<ReportParty>();

		public void Merge(ActivityData? newData)
		{
			if (newData == null) return;

			// Assuming merging means combining the lists
			if (newData.ReportParties != null && newData.ReportParties.Count > 0)
			{
				foreach (var newParty in newData.ReportParties)
				{
					var existingParty = ReportParties.Find(p => p.Role == newParty.Role);
					if (existingParty != null)
					{
						existingParty.Merge(newParty);
					}
					else
					{
						ReportParties.Add(newParty);
					}
				}
			}
		}

		public class ReportParty
		{
			public string? Role { get; set; } = "0";
			public PersonData? Person { get; set; } = new PersonData();
			public string? Country { get; set; }
			public string? Reason { get; set; }
			public string? Comments { get; set; }

			public void Merge(ReportParty? newParty)
			{
				if (newParty == null) return;

				Role = newParty.Role ?? Role;
				Country = newParty.Country ?? Country;
				Reason = newParty.Reason ?? Reason;
				Comments = newParty.Comments ?? Comments;

				if (newParty.Person != null)
				{
					Person = Person ?? new PersonData();
					Person.Merge(newParty.Person);
				}
			}

			public class PersonData
			{
				public string? Gender { get; set; }
				public string? Title { get; set; }
				public string? FirstName { get; set; }
				public string? LastName { get; set; }
				public DateTime? Birthdate { get; set; }
				public string? BirthPlace { get; set; }
				public string? Nationality1 { get; set; }
				public List<AddressData> Addresses { get; set; } = new List<AddressData>();
				public List<EmailData> Emails { get; set; } = new List<EmailData>();
				public string? Occupation { get; set; }
				public List<IdentificationData> Identifications { get; set; } = new List<IdentificationData>();
				public string? SourceOfWealth { get; set; }

				public void Merge(PersonData? newPerson)
				{
					if (newPerson == null) return;

					Gender = newPerson.Gender ?? Gender;
					Title = newPerson.Title ?? Title;
					FirstName = newPerson.FirstName ?? FirstName;
					LastName = newPerson.LastName ?? LastName;
					Birthdate = newPerson.Birthdate != default(DateTime) ? newPerson.Birthdate : Birthdate;
					BirthPlace = newPerson.BirthPlace ?? BirthPlace;
					Nationality1 = newPerson.Nationality1 ?? Nationality1;
					Occupation = newPerson.Occupation ?? Occupation;
					SourceOfWealth = newPerson.SourceOfWealth ?? SourceOfWealth;

					// Merge Addresses
					for (int i = 0; i < newPerson.Addresses.Count; i++)
					{
						if (i < Addresses.Count)
						{
							Addresses[i].Merge(newPerson.Addresses[i]);
						}
						else
						{
							Addresses.Add(newPerson.Addresses[i]);
						}
					}

					// Simply replace or add emails from the new person
					if (newPerson.Emails != null && newPerson.Emails.Count > 0)
					{
						Emails = new List<EmailData>(newPerson.Emails);
					}

					// Merge Identifications
					for (int i = 0; i < newPerson.Identifications.Count; i++)
					{
						if (i < Identifications.Count)
						{
							Identifications[i].Merge(newPerson.Identifications[i]);
						}
						else
						{
							Identifications.Add(newPerson.Identifications[i]);
						}
					}
				}

				public class AddressData
				{
					public string? AddressType { get; set; }
					public string? Address { get; set; }
					public string? City { get; set; }
					public string? Zip { get; set; }
					public string? CountryCode { get; set; }

					public void Merge(AddressData? newAddress)
					{
						if (newAddress == null) return;

						AddressType = newAddress.AddressType ?? AddressType;
						Address = newAddress.Address ?? Address;
						City = newAddress.City ?? City;
						Zip = newAddress.Zip ?? Zip;
						CountryCode = newAddress.CountryCode ?? CountryCode;
					}
				}

				public class IdentificationData
				{
					public string? Type { get; set; }
					public string? Number { get; set; }
					public DateTime? IssueDate { get; set; }
					public DateTime? ExpiryDate { get; set; }
					public string? IssueCountry { get; set; }

					public void Merge(IdentificationData? newIdentification)
					{
						if (newIdentification == null) return;

						Type = newIdentification.Type ?? Type;
						Number = newIdentification.Number ?? Number;
						IssueDate = newIdentification.IssueDate != default(DateTime) ? newIdentification.IssueDate : IssueDate;
						ExpiryDate = newIdentification.ExpiryDate != default(DateTime) ? newIdentification.ExpiryDate : ExpiryDate;
						IssueCountry = newIdentification.IssueCountry ?? IssueCountry;
					}
				}

				public class EmailData
				{
					public string? Email { get; set; }
				}
			}
		}
	}
}
