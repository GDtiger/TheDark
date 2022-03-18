using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserSaveData
{
    [TabGroup("Player", "Info")] public List<PlayerUnitStatus> unitStatus;
    [TabGroup("Player", "Info")] public List<ItemData> inventory = new List<ItemData>();
}