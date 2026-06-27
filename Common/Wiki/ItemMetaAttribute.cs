using System;

namespace Synergia.Common.Wiki;


public enum WeaponType
{
    None,
    Sword,
    Spear,
    Yoyo,
    Bow,
    Gun,
    Wand,
    Tome,
    Minion,
    Sentry,
    Whip,
    Aura,
    Dagger,
    Javelin,
    Boomerang,
    Other,

}

public enum Progression
{
    None,
    PreHardmode,
    Hardmode,
    PostMoonLord,
    Unobtainable
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ItemMetaAttribute : Attribute
{
    public Progression Progression { get; set; } = Progression.None;
    public WeaponType WeaponType { get; set; } = WeaponType.None;

    // Конструктор приймає обов'язкові дані
    public ItemMetaAttribute(params object[] modifiers)
    {
        foreach (object modifier in modifiers)
        {
            switch (modifier)
            {
                case Progression progression:
                    Progression = progression;
                    break;
                case WeaponType weaponType:
                    WeaponType = weaponType;
                    break;
            }
        }
    }
}
