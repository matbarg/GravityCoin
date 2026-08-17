using UnityEngine;

public static class PowerupEffects
{
    // Dreht die Gravity des nächsten Gegners um.
    // "user" ist der Spieler, der das Powerup benutzt hat.
    public static void GravityFlipNearestEnemy(GameObject user)
    {
        PlayerMovement nearest = FindNearestOtherPlayer(user);

        if (nearest != null)
        {
            nearest.ForceGravityFlip();
        }
    }

    // Sucht den nächsten anderen Spieler.
    private static PlayerMovement FindNearestOtherPlayer(GameObject user)
    {
        PlayerMovement[] allPlayers =
            Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        PlayerMovement nearest = null;
        float shortestDistance = Mathf.Infinity;
        Vector2 userPos = user.transform.position;

        foreach (PlayerMovement p in allPlayers)
        {
            
            if (p.gameObject == user) continue;

            float dist = Vector2.Distance(userPos, p.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearest = p;
            }
        }

        return nearest;
    }
}