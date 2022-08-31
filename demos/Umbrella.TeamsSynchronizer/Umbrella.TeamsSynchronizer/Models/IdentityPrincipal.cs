using Umbrella.TeamsSynchronizer.Common;

namespace Umbrella.TeamsSynchronizer.Models;

public class IdentityPrincipal
{
    public string Id { get; set; }
    public string UserPrincipalName { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string JobTitle { get; set; }
    public string Location { get; set; }
    public bool IsGuest { get; set; } = false;

    public IdentityPrincipal() { }

    public IdentityPrincipal(string id, string userPrincipalName, string displayName)
    {
        Id = id;
        UserPrincipalName = userPrincipalName;
        DisplayName = displayName;
    }

    public static IdentityPrincipal CreateFakeIdentity()
    {
        var randomTextGenerator = new RandomGenerator();

        var id = Guid.NewGuid().ToString();
        var upn = $"{randomTextGenerator.RandomString(8)}@marvel.onmicrosoft.com";
        var displayName = $"{randomTextGenerator.RandomString(4)} {randomTextGenerator.RandomString(8)}";

        var identity = new IdentityPrincipal(id, upn, displayName)
        {
            IsGuest = false,
            Email = upn,
            JobTitle = randomTextGenerator.RandomString(8),
            Location = randomTextGenerator.RandomString(10)
        };

        return identity;
    }
}
