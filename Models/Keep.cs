namespace keepr.Models;

public class Keep
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Img { get; set; } = "https://images.ctfassets.net/b4k16c7lw5ut/TdKLskzZYA5IJddHkSvQE/23850e128cb6c060851b2371ccfabef3/Clipchamp-Pinterest-partnership-step-3.jpg?w=1000&h=563&fl=progressive&q=50&fm=jpg";
    public int? Views { get; set; }

    public int Kept { get; set; }
    public string CreatorId { get; set; }
    public Account Creator { get; set; }
}

public class KeepInVault : Keep
{
    public int vaultKeepId { get; set; }
}
