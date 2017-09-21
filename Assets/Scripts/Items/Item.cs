using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Represent the power and the rarity of an Item
    /// TODO: maybe add or remove some rarity
    /// </summary>
    protected enum ERarity
    {
        Common,         //
        Magic,          //not so bad
        Rare,           //medium
        Epic,           //good
        Legendary       //extra
    }


    /// <summary>
    /// Represents the name of existing characteristic
    /// TODO: Create a class characteristic
    /// TODO: maybe change or add some characteristic
    /// TODO: differenciate vita, strength, degat crit, chance crit comme wow ?
    /// </summary>
    public enum ECharacteristic
    {
        Vitality,
        Strength,

    }

    private Dictionary<ERarity, double> _coefficientDueToRarity;
    private Dictionary<ECharacteristic, int> _characteristics;

    /// <summary>
    /// The item level
    /// More the level is high, more the item has mark  
    /// </summary>
    private readonly int _itemLevel;

    private readonly ERarity _rarity;


    /// <summary>
    /// Property readonly
    /// Get the dictionnary of all characteristic
    /// </summary>
    public Dictionary<ECharacteristic, int> Characteristics
    {
        get
        {
            return _characteristics;
        }
    }

    /// <summary>
    /// Property readonly
    /// Get the rarity of Item
    /// </summary>
    protected ERarity Rarity
    {
        get
        {
            return _rarity;
        }
    }

    /// <summary>
    /// Property readonly
    /// Get the level of Item
    /// </summary>
    public int ItemLevel
    {
        get
        {
            return _itemLevel;
        }

    }

    /// <summary>
    /// abstract constructor to create an Item
    /// </summary>
    /// <param name="itemLevel">The level of Item</param>
    /// <param name="rarity">The rarity of Item</param>
    protected Item(int itemLevel, ERarity rarity)
    {
        _itemLevel = itemLevel;
        _rarity = rarity;
        _coefficientDueToRarity = new Dictionary<ERarity, double>();
        InitCoefficient();
        _characteristics = new Dictionary<ECharacteristic, int>();
    }


    protected Item RandomItem(int itemLevel)
    {
        //TODO : get random piece
        //TODO : get random rarity
        return new Item(25, ERarity.Common);
    }

    public int NumberOfMark
    {
        get
        {
            var totalMark = MarkDueToLevel(ItemLevel) * _coefficientDueToRarity[Rarity];

            return Convert.ToInt32(Math.Round(totalMark));
        }
    }

    abstract protected int MarkDueToLevel(int level);

    private void InitCoefficient()
    {
        _coefficientDueToRarity.Add(ERarity.Common, 1);
        _coefficientDueToRarity.Add(ERarity.Magic, 1.1);
        _coefficientDueToRarity.Add(ERarity.Rare, 1.2);
        _coefficientDueToRarity.Add(ERarity.Epic, 1.5);
        _coefficientDueToRarity.Add(ERarity.Legendary, 1.6);
    }

    protected int RestantMark
    {
        get
        {
            return NumberOfMark - AttribuatedMark;
        }
    }

    protected int AttribuatedMark
    {
        get
        {
            var attribuatedMark = 0;
            foreach (var keyValue in Characteristics)
            {
                attribuatedMark = keyValue.Value;
            }

            return attribuatedMark;
        }
    }

    protected void SetCharacteristic(ECharacteristic characteristic, int mark)
    {
        if (mark > RestantMark)
        {
            return;
        }

        if (!Characteristics.ContainsKey(characteristic))
        {
            Characteristics.Add(characteristic, mark);
        }
        else
        {
            Characteristics[characteristic] += mark;
        }
    }

    protected int ValueOfCharacteristic(ECharacteristic characteristic)
    {
        var value = 0;

        if (Characteristics.ContainsKey(characteristic))
        {
            value = Characteristics[characteristic];
        }

        return value;
    }
}
