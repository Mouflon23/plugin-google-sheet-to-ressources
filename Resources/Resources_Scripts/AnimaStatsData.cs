using System;
using Godot;

[GlobalClass, Tool]
public partial class AnimaStatsData : Resource
{
    [Export]
    public string AnimaId;

    [Export]
    public string AnimaName;

    [Export]
    public int Cost;

    [Export]
    public int MaxHealth;

    [Export]
    public float AttackSpeed = 1f;

    // --> A terme : passer en "nb d'attaque par seconde" plutot que "délai entre chaque attaque"
    [Export]
    public string TalentUid;

    [Export(PropertyHint.MultilineText)]
    public string TalentDescription;

    [Export]
    public Godot.Collections.Dictionary<string, int> Cards = new();
}
