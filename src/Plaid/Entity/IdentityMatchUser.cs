namespace Going.Plaid.Entity;

/// <summary>
/// <para>The user's legal name, phone number, email address and address used to perform fuzzy match.</para>
/// </summary>
public class IdentityMatchUser
{
	/// <summary>
	/// <para>The user's full legal name.</para>
	/// </summary>
	[JsonPropertyName("legal_name")]
	public string? LegalName { get; set; } = default!;

	/// <summary>
	/// <para>The user's phone number, in E.164 format: +{countrycode}{number}. For example: "+14151234567". Phone numbers provided in other formats will be parsed on a best-effort basis.</para>
	/// </summary>
	[JsonPropertyName("phone_number")]
	public string? PhoneNumber { get; set; } = default!;

	/// <summary>
	/// <para>The user's email address.</para>
	/// </summary>
	[JsonPropertyName("email_address")]
	public string? EmailAddress { get; set; } = default!;

	/// <summary>
	/// <para>Data about the components comprising an address.</para>
	/// </summary>
	[JsonPropertyName("address")]
	public Entity.AddressData? Address { get; set; } = default!;
}