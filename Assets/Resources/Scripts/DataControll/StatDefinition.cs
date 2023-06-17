using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "ScriptableObjects/Stat")]
public class StatDefinition : ScriptableObject
{
    [SerializeField] private int _baseValue;
    [SerializeField] private int _cap;

    public int BaseValue => _baseValue;
    public int Cap => _cap;
}
