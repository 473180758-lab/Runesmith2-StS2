using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using Runesmith2.Runesmith2Code.Structs;

namespace Runesmith2.Runesmith2Code.Extensions;

public static class ModelModifyElementsExtension
{
    public static bool TryModifyElementsCost(this BrilliantScarf model, CardModel card, Elements originalCost,
        out Elements modifiedCost)
    {
        modifiedCost = originalCost;
        if (!model.ShouldModifyCost(card))
        {
            return false;
        }
        modifiedCost = new Elements(0);
        return true;
    }
    
    public static bool TryModifyElementsCost(this VoidFormPower model, CardModel card, Elements originalCost,
        out Elements modifiedCost)
    {
        modifiedCost = originalCost;
        if (model.ShouldSkip(card))
        {
            return false;
        }
        modifiedCost = new Elements(0);
        return true;
    }
}