using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Abstract class for Item
/// </summary>
public abstract class Item : MonoBehaviour {

    #region Enum
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
    #endregion

    #region private Data
    /// <summary>
    /// The item level
    /// More the level is high, more the item has mark  
    /// </summary>
    private int _itemLevel;

    /// <summary>
    /// The rarity of Item
    /// </summary>
    private ERarity _rarity;
    #endregion

    // Use this for initialization
    void Start (int itemLevel,ERarity rarity) {
        _itemLevel = itemLevel;
        _rarity = rarity;
        _coefficientDueToRarity = new Dictionary<ERarity, double>();
        InitCoefficient();
        _characteristics = new Dictionary<ECharacteristic, int>();
    }
	
    private Dictionary<ERarity, double> _coefficientDueToRarity;
    private Dictionary<ECharacteristic, int> _characteristics;
    
    #region Public property
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
    /// Get the number of mark of an item.
    /// Each mark can be converted into characteristic
    /// </summary>
    public int NumberOfMark
    {
        get
        {
            var totalMark = MarkDueToLevel(ItemLevel) * _coefficientDueToRarity[Rarity];

            return Convert.ToInt32(Math.Round(totalMark));
        }
    }

    /// <summary>
    /// Represents the number of mark which can be converted into characteristic.
    /// </summary>
    protected int RestantMark
    {
        get
        {
            return NumberOfMark - AttribuatedMark;
        }
    }

    /// <summary>
    /// Represents the number of mark ever converted into characteristic.
    /// </summary>
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
    #endregion


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

   
    abstract protected int MarkDueToLevel(int level);

    private void InitCoefficient()
    {
        _coefficientDueToRarity.Add(ERarity.Common, 1);
        _coefficientDueToRarity.Add(ERarity.Magic, 1.1);
        _coefficientDueToRarity.Add(ERarity.Rare, 1.2);
        _coefficientDueToRarity.Add(ERarity.Epic, 1.5);
        _coefficientDueToRarity.Add(ERarity.Legendary, 1.6);
    }

   

    /// <summary>
    /// Convert mark from Item into characteristic for this Item.
    /// </summary>
    /// <param name="characteristic"></param>
    /// <param name="mark"></param>
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

    /// <summary>
    /// Return the number of mark convert into the <param name="characteristic"/>
    /// </summary>
    /// <param name="characteristic">The characteristic.</param>
    /// TODO : rename method
    /// <returns></returns>
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
