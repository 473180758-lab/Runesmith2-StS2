using BaseLib.Audio;

namespace Runesmith2.Runesmith2Code.Utils;

public static class RunesmithModSounds
{
    public static readonly ModSound GrindstoneSfx = new("res://Runesmith2/audio/grindstone_sharpen.ogg");

    public static void PlayGrindStoneSfx()
    {
        GrindstoneSfx.Play(pitchVariation: 0.1f);
    }
}