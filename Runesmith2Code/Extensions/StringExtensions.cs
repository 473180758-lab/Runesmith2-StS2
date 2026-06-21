namespace Runesmith2.Runesmith2Code.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{
    public static string ToRes(this string path)
    {
        return Path.Join("res://", path);
    }

    public static string ImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", path);
    }

    public static string CardImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "card_portraits", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "card_portraits", "big", path);
    }

    public static string EnchantmentImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "enchantments", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "powers", path);
    }

    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "powers", "big", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "relics", "big", path);
    }

    public static string PotionImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "potions", path);
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "charui", path);
    }

    public static string RuneImagePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "images", "runes", "icons", path + ".png");
    }

    public static string RuneScenePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "scenes", "runes", "rune_visuals", path + ".tscn");
    }

    public static string TopPanelScenePath(this string path)
    {
        return Path.Join(Runesmith2Mod.ModId, "scenes", "top_panel", path + ".tscn");
    }


    public static string ScenePath(this string path, string dir)
    {
        return Path.Join(Runesmith2Mod.ModId, "scenes", dir, path + ".tscn");
    }
}