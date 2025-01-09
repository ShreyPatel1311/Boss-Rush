using UnityEngine;

[CreateAssetMenu(fileName = "DragonkinConfigurationSO", menuName = "Scriptable Objects/DragonkinConfigurationSO")]
public class DragonkinConfigurationSO : ScriptableObject
{
    [Header("Distance")]
    public float lightAttackDistance;
    public float ComboAttackDistance;
    public float LongRangeAttackDistance;

    [Header("Animations")]
    public float animationSpeedP1;
    public float animationSpeedP2;

    [Header("Nav Agent")]
    public float navSpeedP1;
    public float navSpeedP2;
    public float navAngularSpeed;
    public float navAccelartion;
}
