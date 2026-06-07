#region

using Runesmith2.Runesmith2Code.Extensions;

#endregion

namespace Runesmith2.Runesmith2Code.Nodes.Runes;

public partial class NRuneVisualsReplace : NRuneVisuals
{
    protected override MixBlend TriggerMixBlend => MixBlend.MixBlend_Replace;
}