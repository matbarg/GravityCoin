using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerCombat combat;

    void Awake()
    {
        combat = GetComponentInParent<PlayerCombat>();
    }

    public void EndAttack()
    {
        combat.EndAttack();
    }
}
