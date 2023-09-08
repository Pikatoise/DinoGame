using System;
using System.Collections.Generic;

namespace DinoGame.Models;

public partial class Player
{
    public long Id { get; set; }

    public string Nickname { get; set; } = null!;

    public long? Score { get; set; }
}
