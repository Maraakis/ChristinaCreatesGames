using UnityEngine;
using ChristinaCreatesGames.UI;
using TMPro;


//* THIS IS SUPER PROTOTYPE-Y CODE, USE AT YOUR OWN RISK
namespace ChristinaCreatesGames.Prototype
{
    public class InventoryManager : MonoBehaviour
    {
        public ResourceSlot[] resourceSlots;
        public ModifierButton[] modifierButtons;
        public TMP_Text amountText;

        public void HandleSlotClick(ResourceSlot slot)
        {
            if (!slot.isFilled) return;
            
            TakeOne(slot);
        }
        
        public void HandleSlotModifierAClick(ResourceSlot slot)
        {
            if (!slot.isFilled) return;
            
            SplitInHalf(slot);
        }
        
        public void HandleSlotModifierBClick(ResourceSlot slot)
        {
            if (!slot.isFilled) return;
            
            TakeTen(slot);
        }
        
        public void HandleSlotModifierBothClick(ResourceSlot slot)
        {
            if (!slot.isFilled) return;
            
            TakeAll(slot);
        }
        
        public void HandleSlotModifierAHeldDown()
        {
            amountText.SetText("Split in half");
        }
        
        public void HandleSlotModifierBHeldDown()
        {
            amountText.SetText("Take ten");
        }
        
        public void HandleSlotModifierBothHeldDown()
        {
            amountText.SetText("Take all");
        }
        
        public void HandleSlotNoModifierHeldDown()
        {
            amountText.SetText("Take one");
        }

        private void Start()
        {
            modifierButtons = FindObjectsOfType<ModifierButton>();

            if (modifierButtons.Length == 0)
            {
                Debug.LogError("No ModifierButtons found in the scene. Please ensure they are correctly added.");
                return;
            }

            foreach (ModifierButton button in modifierButtons)
            {
                ResourceSlot slot = button.GetComponent<ResourceSlot>();
                if (slot == null)
                {
                    Debug.LogError($"ModifierButton '{button.name}' is missing a ResourceSlot component.");
                    continue;
                }

                button.OnClick.AddListener(() => HandleSlotClick(slot));
                button.OnConditionalClick_A.AddListener(() => HandleSlotModifierAClick(slot));
                button.OnConditionalClick_B.AddListener(() => HandleSlotModifierBClick(slot));
                button.OnConditionalClick_Both.AddListener(() => HandleSlotModifierBothClick(slot));
                button.OnConditionalHeldDown_A.AddListener(HandleSlotModifierAHeldDown);
                button.OnConditionalHeldDown_B.AddListener(HandleSlotModifierBHeldDown);
                button.OnConditionalHeldDown_Both.AddListener(HandleSlotModifierBothHeldDown);
                button.OnConditionalHeldDown_None.AddListener(HandleSlotNoModifierHeldDown);
            }
        }

        private void TakeAll(ResourceSlot fromSlot)
        {
            int amountToTransfer = fromSlot.Amount;
            fromSlot.Amount = 0;
            DistributeResource(amountToTransfer, fromSlot);
        }

        private void SplitInHalf(ResourceSlot fromSlot)
        {
            int halfAmount = Mathf.CeilToInt(fromSlot.Amount / 2f);
            fromSlot.Amount -= halfAmount;
            DistributeResource(halfAmount, fromSlot);
        }

        private void TakeTen(ResourceSlot fromSlot)
        {
            int amountToTransfer = Mathf.Min(10, fromSlot.Amount);
            fromSlot.Amount -= amountToTransfer;
            DistributeResource(amountToTransfer, fromSlot);
        }

        private void TakeOne(ResourceSlot fromSlot)
        {
            int amountToTransfer = 1;
            fromSlot.Amount -= amountToTransfer;
            DistributeResource(amountToTransfer, fromSlot);
        }

        private void DistributeResource(int amount, ResourceSlot fromSlot)
        {
            foreach (var slot in resourceSlots)
            {
                if (slot == fromSlot) continue;

                int spaceAvailable = slot.MaxAmount - slot.Amount;

                if (spaceAvailable > 0)
                {
                    if (amount <= spaceAvailable)
                    {
                        slot.Amount += amount;
                        return;
                    }

                    slot.Amount += spaceAvailable;
                    amount -= spaceAvailable;
                }
            }

            if (amount > 0)
                Debug.LogWarning("Not enough space in inventory to distribute the remaining amount!");
        }
    }
}