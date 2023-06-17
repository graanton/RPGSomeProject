using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class Stat
{
    protected StatDefinition _definition;
    protected float _value;
    protected List<StatModifire> _mods;

    public float Value => _value;
    public event Action ValueChanged;

    public Stat(StatDefinition statDefinition)
    {
        _definition = statDefinition;
        _value = _definition.BaseValue;
    }

    public void AddModifire(StatModifire modifire)
    {
        _mods.Add(modifire);
        CulculateValue();
    }

    public void RemoveModifireFromSource(Object source)
    {
        _mods = _mods.Where(m => m.Source.GetInstanceID() != source.GetInstanceID()).ToList();
        CulculateValue();
    }

    protected virtual void CulculateValue()
    {
        float value = _definition.BaseValue;

        _mods.Sort((x, y) => x.Type.CompareTo(y.Type));

        foreach (StatModifire mod in _mods)
        {
            switch (mod.Type)
            {
                case StatModifyType.Addive:
                    value += mod.Magnitude;
                    break;
                case StatModifyType.Multiply:
                    value *= mod.Magnitude;
                    break;
                case StatModifyType.Override:
                    value = mod.Magnitude;
                    break;
            }

            value = Mathf.Min(value, _definition.Cap);

            if (_value != value)
            {
                _value = value;
                ValueChanged?.Invoke();
            }
        }
    }
}

public enum StatModifyType
{
    Addive,
    Multiply,
    Override
}

public class StatModifire
{
    private readonly float _magnitude;
    private readonly StatModifyType _type;
    private readonly Object _source;

    public float Magnitude => _magnitude;
    public StatModifyType Type => _type;
    public Object Source => _source;

    public StatModifire(float magnitude, StatModifyType type, Object source)
    {
        _magnitude = magnitude;
        _type = type;
        _source = source;
    }
}
