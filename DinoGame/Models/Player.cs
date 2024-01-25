namespace DinoGame.Models;

public class Player
{
    public long Id { get; set; }

    public string Nickname { get; set; } = null!;

    public long? Score { get; set; }
}
