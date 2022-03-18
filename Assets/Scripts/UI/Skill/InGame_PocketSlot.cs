using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class InGame_PocketSlot : MonoBehaviour
{
    [TabGroup("Slot", "Slot")] public int slotNum;
    //[TabGroup("Slot", "Need")] [SerializeField] protected Image icon;
    [TabGroup("Slot", "Need")] [SerializeField] protected TextMeshProUGUI countText;
    [TabGroup("Slot", "Auto")] public Transform showActiveSkillIcon;
    [TabGroup("Slot", "Auto")] [SerializeField] protected GameManager gm;

    public virtual void Initiate()
    {
        //if (inventoryManager == null) inventoryManager = Town_InventoryManager.Instance;
        if (gm == null) gm = GameManager.Instance;
    }

    public void AcctiveButton()
    {
        showActiveSkillIcon.gameObject.SetActive(true);
    }
    public void DeactiveButton()
    {
        showActiveSkillIcon.gameObject.SetActive(false);
    }
}
