using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData
{
    public List<ItemInfo> allData;
    public List<ItemInfo> equipData;
    public List<ItemInfo> propData;
    public List<ItemInfo> consumablesData;

    public ShopData()
    {
        allData = new List<ItemInfo>() 
        { 
            new ItemInfo() { id = 1, amount = 1},
            new ItemInfo() { id = 2, amount = 1},
            new ItemInfo() { id = 3, amount = 1}, 
            new ItemInfo() { id = 4, amount = 1},
            new ItemInfo() { id = 5, amount = 1}, 
            new ItemInfo() { id = 6, amount = 1},
            new ItemInfo() { id = 7, amount = 1}, 
            new ItemInfo() { id = 8, amount = 1},
            new ItemInfo() { id = 9, amount = 1}, 
            new ItemInfo() { id = 10, amount = 1},
            new ItemInfo() { id = 11, amount = 1}, 
            new ItemInfo() { id = 12, amount = 1},
            new ItemInfo() { id = 13, amount = 1}, 
            new ItemInfo() { id = 14, amount = 1},
            new ItemInfo() { id = 15, amount = 1}
        };

        equipData = new List<ItemInfo>()
        {
            new ItemInfo() { id = 1, amount = 1 },
            new ItemInfo() { id = 2, amount = 1 },
            new ItemInfo() { id = 3, amount = 1 },
            new ItemInfo() { id = 4, amount = 1 },
            new ItemInfo() { id = 5, amount = 1 },
            new ItemInfo() { id = 6, amount = 1 },
            new ItemInfo() { id = 7, amount = 1 },
            new ItemInfo() { id = 8, amount = 1 }
        };

        propData = new List<ItemInfo>()
        {
            new ItemInfo() { id = 9, amount = 1},
            new ItemInfo() { id = 10, amount = 1},
            new ItemInfo() { id = 11, amount = 1},
            new ItemInfo() { id = 12, amount = 1}
        };

        consumablesData = new List<ItemInfo>()
        {
            new ItemInfo() { id = 13, amount = 1},
            new ItemInfo() { id = 14, amount = 1},
            new ItemInfo() { id = 15, amount = 1}
        };
    }
}
