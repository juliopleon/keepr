namespace keepr.Models;

// public class Profile : RepoItem<string>
// {
//   public string Name { get; set; }
//   public string Picture { get; set; }
// }

public class Account
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Picture { get; set; }

    public string CoverImg { get; set; }

    // public string ProfileId { get; set; }
}
