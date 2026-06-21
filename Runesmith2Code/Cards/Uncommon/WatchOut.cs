#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.DynamicVars;
using Runesmith2.Runesmith2Code.Powers;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Uncommon;

public class WatchOut : Runesmith2Card
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public WatchOut() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
    {
        WithPower<BracePower>(5, 2);
        WithVar(new EnhanceByVar(1));
        WithCards(2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        await CommonActions.Apply<BracePower>(choiceContext, play.Target, this);

        var targetPlayer = play.Target.Player;
        if (play.Target.IsPlayer && targetPlayer != null)
        {
            await RunesmithCardCmd.EnhanceRandomCards(choiceContext, Owner, PileType.Hand.GetPile(targetPlayer).Cards,
                DynamicVars.Cards.IntValue, DynamicVars[EnhanceByVar.defaultName].IntValue,
                Owner.RunState.Rng.CombatCardSelection);
        }
    }
}