#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Runesmith2.Runesmith2Code.Combat;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.DynamicVars;
using Runesmith2.Runesmith2Code.Hooks;
using Runesmith2.Runesmith2Code.HoverTips;
using Runesmith2.Runesmith2Code.Structs;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Uncommon;

public class WhiteBalance : Runesmith2Card
{
    public WhiteBalance() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVar(new ElementsVar(1));
        WithCalculatedDamage(0, 2, (card, _) =>
        {
            if (card.CombatState == null) return 0;
            
            var baseElements = RunesmithHook.ModifyElementsGain(card.CombatState, card.Owner,
                new Elements(card.DynamicVars[ElementsVar.defaultName].IntValue),
                ValueProp.Move, card, out var _).Total; 
            
            return GetElementsGainedThisTurn(card) + baseElements;
        }, ValueProp.Move, 0, 1);
        WithTip(RunesmithHoverTip.Elements);
    }

    private static int GetElementsGainedThisTurn(CardModel card)
    {
        return CombatManager.Instance.History.Entries
            .OfType<ElementsModifiedEntry>()
            .Where(eme =>
                eme.HappenedThisTurn(card.CombatState) && card.Owner == eme.Player && eme.Amount.Total > 0)
            .Select(eme => eme.Amount)
            .Aggregate(new Elements(0), (e1, e2) => e1 + e2)
            .Total;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);

        var damageToDeal = DynamicVars.CalculatedDamage.Calculate(play.Target);
        
        await RunesmithPlayerCmd.GainElements(new Elements(DynamicVars[ElementsVar.defaultName].IntValue), Owner, play);

        await DamageCmd.Attack(damageToDeal)
            .FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
}