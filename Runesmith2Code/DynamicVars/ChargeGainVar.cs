#region

using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace Runesmith2.Runesmith2Code.DynamicVars;

public class ChargeGainVar : DynamicVar
{
    public const string defaultName = "ChargeGain";

    public ChargeGainVar(int charge, bool tip = true)
        : this(defaultName, charge, tip)
    {
    }

    public ChargeGainVar(string name, int charge, bool tip)
        : base(name, charge)
    {
        if (tip) this.WithTooltip("RUNESMITH2-CHARGE");
    }
}