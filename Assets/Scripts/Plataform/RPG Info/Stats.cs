using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour, IStats
{

    private int currentLife;
    private int currentMana;

    public StatsValue life;

    public StatsValue mana;

    public StatsValue damage;

    public StatsValue defense;

    public StatsValue speed;

    private void Start()
    {
        currentLife = life.GetValue();
        currentMana = mana.GetValue();
    }

    public void BonusStats(int value, STATUS status, string key, bool add)
    {
        switch (status)
        {
            case STATUS.DAMAGE:
                damage.ChangeModifier(key, value, add);
                break;
            case STATUS.DEFENSE:
                defense.ChangeModifier(key, value, add);
                break;
            case STATUS.MANA:
                mana.ChangeModifier(key, value, add);
                break;
            case STATUS.SPEED:
                speed.ChangeModifier(key, value, add);
                break;
            case STATUS.LIFE:
                life.ChangeModifier(key, value, add);
                break;
        }
    }

    public void RecoverLife(int value)
    {
        if (currentLife + value < life.GetValue())
            currentLife += value;
        else currentLife = life.GetValue();
    }

    public void RecoverMana(int value)
    {
        if (currentMana + value < mana.GetValue())
            currentMana += value;
        else currentMana = mana.GetValue();
    }

    public bool ConsumeMana(int value)
    {
        if (currentMana - value > 0)
        {
            if (currentMana - value < 0)
                currentMana -= value;
            else currentMana = 0;

            return true;
        }
        return false;
    }

    public void TakeDamage(int value)
    {
        if (currentLife - value > 0)
        {
            currentLife -= value;
            Debug.Log(gameObject.name + $" Tomou {value} dano");
        }
        else
        {
            currentLife = 0;
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}

[Serializable]
public class StatsValue
{
    [SerializeField]
    public int Value;
    public Dictionary<string, int> modifiers = new Dictionary<string, int>();

    public int GetValue()
    {
        int values = 0;

        foreach (var modifier in modifiers)
        {
            values += modifier.Value;
        }

        return Value + values;

    }

    public void ChangeModifier(string modifierId, int value, bool add)
    {
        if (add)
        {
            modifiers.Add(modifierId, value);
        }
        else
        {
            modifiers.Remove(modifierId);
        }
    }


}


public interface IStats
{
    void TakeDamage(int value);

    void RecoverLife(int value);

    void RecoverMana(int value);

    bool ConsumeMana(int value);

    void BonusStats(int value, STATUS status, string key, bool add);

    void Die();

}

public enum STATUS
{
    DAMAGE, DEFENSE, SPEED, MANA, LIFE
}