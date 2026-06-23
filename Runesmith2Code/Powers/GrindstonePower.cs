#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.Extensions;
using Runesmith2.Runesmith2Code.Nodes.Vfx;
using Runesmith2.Runesmith2Code.Utils;

#endregion

namespace Runesmith2.Runesmith2Code.Powers;

public class GrindstonePower : Runesmith2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private class Data
    {
        public CardModel? OwnerCard;
    }

    protected override object InitInternalData()
    {
        return new Data();
    }

    public void SetOwnerCard(CardModel card)
    {
        GetInternalData<Data>().OwnerCard = card;
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Player == null) return;
        var card = cardPlay.Card;
        if (card.Owner != Owner.Player) return;
        var internalData = GetInternalData<Data>();
        if (card == internalData.OwnerCard)
        {
            // Prevent Grindstone from upgrading itself immediately after play.
            internalData.OwnerCard = null;
            return;
        }

        var hasActivated = false;
        if (card.IsUpgradable)
        {
            CardCmd.Upgrade(card);
            Flash();
            hasActivated = true;
        }
        else if (card.CanEnhance())
        {
            await RunesmithCardCmd.Enhance(choiceContext, Owner.Player, card, cardPlay, 1, true);
            Flash();
            hasActivated = true;
        }

        if (RunesmithConfig.EnableGrindstoneVfx && hasActivated && cardPlay.ResultPile != PileType.None)
        {
            var cardNode = NCard.FindOnTable(card);
            if (cardNode == null) return;

            var vfx = NCardGrindstoneVfx.Create(cardNode, card);
            if (vfx == null) return;
            
            RunesmithModSounds.PlayGrindStoneSfx();
            await vfx.PlayAnimation(true);
            await Cmd.CustomScaledWait(0.25f, 0.4f);
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
            await PowerCmd.Decrement(this);
    }
}