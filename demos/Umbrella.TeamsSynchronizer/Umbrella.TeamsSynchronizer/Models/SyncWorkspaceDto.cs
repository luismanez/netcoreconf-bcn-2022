using System.Text.Json;
using Umbrella.TeamsSynchronizer.Common;

namespace Umbrella.TeamsSynchronizer.Models;

public class SyncWorkspaceDto
{
    public Guid WorkspaceId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime LastUpdatedDateTime { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string SiteId { get; set; }
    public string SiteAbsoluteUrl { get; set; }
    public Guid GroupId { get; set; }
    public string GroupType { get; set; }
    public string Mail { get; set; }
    public string MailNickname { get; set; }
    public bool IsPublic { get; set; }
    public string TeamsInternalId { get; set; }
    public string MainLink { get; set; }
    public IdentityPrincipal CreatedByUser { get; set; }
    public bool IsArchived { get; set; }
    public List<IdentityPrincipal> Owners { get; set; }
    public List<IdentityPrincipal> Members { get; set; }
    public List<IdentityPrincipal> Visitors { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public void UpdateWorkspaceWithGraphInfo()
    {
        var random = new Random();

        var randomTextGenerator = new RandomGenerator();

        GroupId = Guid.NewGuid();
        LastUpdatedDateTime = DateTime.Now;
        MailNickname = randomTextGenerator.RandomString(5);
        Mail = $"{MailNickname}@onmicrosoft.com";
        Description = randomTextGenerator.RandomString(3);
        Title = randomTextGenerator.RandomString(10);
        CreatedByUser = IdentityPrincipal.CreateFakeIdentity();
        TeamsInternalId = randomTextGenerator.RandomString(10);
        MainLink =
            "https://marvel.sharepoint.com/sites/123";
        CreatedByUserId = Guid.Parse(CreatedByUser.Id);
        Owners = GenerateListOfFakeUsers(random.Next(2, 10));
        Members = GenerateListOfFakeUsers(random.Next(10, 65));
        Visitors = GenerateListOfFakeUsers(random.Next(50, 100));
    }

    private static List<IdentityPrincipal> GenerateListOfFakeUsers(int size)
    {
        var users = new List<IdentityPrincipal>();
        for (var i = 0; i < size; i++)
        {
            users.Add(IdentityPrincipal.CreateFakeIdentity());
        }

        return users;
    }

    public void UpdateWorkspaceWithSharePointInfo()
    {
        SiteId = Guid.NewGuid().ToString();
        LastUpdatedDateTime = DateTime.Now;
        SiteAbsoluteUrl = $"https://marvel.sharepoint.com/sites/{MailNickname}";
    }

    public static SyncWorkspaceDto NewFakeWorkspaceDto()
    {
        return new SyncWorkspaceDto
        {
            WorkspaceId = Guid.NewGuid(),
            IsPublic = true,
            CreatedDateTime = DateTime.Now,
            LastUpdatedDateTime = DateTime.MinValue,
            IsArchived = false
        };
    }
}