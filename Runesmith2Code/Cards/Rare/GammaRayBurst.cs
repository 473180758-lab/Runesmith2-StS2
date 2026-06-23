#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.Extensions;
using Runesmith2.Runesmith2Code.HoverTips;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Rare;

public class GammaRayBurst : Runesmith2Card
{
    public GammaRayBurst() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Amount", 0, 1);
        WithTip(RunesmithHoverTip.Break);
        WithTip(RunesmithHoverTip.Charge);
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var xValue = ResolveEnergyXValue() + DynamicVars["Amount"].IntValue;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        
        var runeQueue = Owner.PlayerCombatState?.GetRuneQueue();
        if (runeQueue == null || !runeQueue.HasAny()) return;
        
        for (var i = 0; i < xValue; i++)
        {
            if (runeQueue.Runes.All(r => !r.CanPassive)) break;
            await RuneCmd.PassiveAll(choiceContext, Owner);
        }
        
        var index = 0;
        while (index < runeQueue.Runes.Count)
        {
            var currRune = runeQueue.Runes[index];
            if (currRune.ChargeVal == 0)
            {
                await RuneCmd.Break(choiceContext, Owner, currRune);
                await Cmd.CustomScaledWait(0.1f, 0.2f);
            }
            else
            {
                // increment index as rune wasn't broken
                index++;
            }
        }
    }
}