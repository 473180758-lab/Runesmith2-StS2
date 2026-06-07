#region

using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using Runesmith2.Runesmith2Code.Extensions;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Rare;

public class Superposition : Runesmith2Card
{
    public Superposition() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(15, 5);
        WithBlock(11, 4);
        WithVar(new DisplayVar<Superposition>("OddEven", c =>
        {
            var runeQueue = c.Owner.PlayerCombatState?.GetRuneQueue();
            var count = 0;
            if (runeQueue != null) count = runeQueue.Runes.Count;

            return count % 2 == 0
                ? new LocString("cards", "RUNESMITH2-SUPERPOSITION.isEven").GetFormattedText()
                : new LocString("cards", "RUNESMITH2-SUPERPOSITION.isOdd").GetFormattedText();
        }));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);

        var runeQueue = Owner.PlayerCombatState?.GetRuneQueue();
        var isOdd = false;
        if (runeQueue != null)
        {
            var count = runeQueue.Runes.Count;
            if (count % 2 != 0) isOdd = true;
        }

        if (isOdd)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                .WithHitCount(2)
                .Targeting(play.Target)
                .SpawningHitVfxOnEachCreature()
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }
        else
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                .Targeting(play.Target)
                .SpawningHitVfxOnEachCreature()
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);

            await CommonActions.CardBlock(this, play);
        }
    }
}