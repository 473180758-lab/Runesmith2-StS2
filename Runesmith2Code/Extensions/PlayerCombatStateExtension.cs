#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using Runesmith2.Runesmith2Code.Entities.Runes;
using Runesmith2.Runesmith2Code.Field;
using Runesmith2.Runesmith2Code.Structs;

#endregion

namespace Runesmith2.Runesmith2Code.Extensions;

public static class PlayerCombatStateExtension
{
    public class RunesmithCombatState(PlayerCombatState combatState, RuneQueue queue)
    {
        public Elements Elements
        {
            get;
            private set
            {
                if (field == value) return;
                var elements = field;
                field = value;
                var state = combatState._player.Creature.CombatState;
                if (state != null)
                    CombatManager.Instance.History.ElementsModified(state,
                        field - elements, combatState._player);
                ElementsChanged?.Invoke(elements, field);
            }
        } = new();

        public RuneQueue RuneQueue => queue;

        public event Action<Elements, Elements>? ElementsChanged;

        public void GainElements(Elements amount)
        {
            if (amount.Total < 0) throw new ArgumentException("Must not be negative", nameof(amount));

            Elements = (Elements + amount).ClampZero();
        }

        public void LoseElements(Elements amount)
        {
            if (amount.Total < 0) throw new ArgumentException("Must not be negative", nameof(amount));

            Elements = (Elements - amount).ClampZero();
        }
    }

    extension(PlayerCombatState playerCombatState)
    {
        public RuneQueue? GetRuneQueue()
        {
            var runesmithCombatState = playerCombatState.Runesmith();
            return runesmithCombatState?.RuneQueue;
        }

        public Elements GetElements()
        {
            var runesmithCombatState = playerCombatState.Runesmith();
            return runesmithCombatState?.Elements ?? new Elements();
        }

        public RunesmithCombatState? Runesmith()
        {
            return RunesmithField.RunesmithCombatState[playerCombatState];
        }
    }
}