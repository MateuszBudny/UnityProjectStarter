using UnityEngine;

public class LookableGeneric : MonoBehaviour, ILookable
{
    [SerializeField]
    private LowkeyEvent onPlayerStartedLookingAt;
    [SerializeField]
    private LowkeyEvent onPlayerKeepsLookingAt;
    [SerializeField]
    private LowkeyEvent onPlayerStoppedLookingAt;

    private IPlayer playerLooking;
    private bool isPlayerLooking = false;
    private bool prevIsPlayerLooking = false;

    private void LateUpdate()
    {
        if(!isPlayerLooking && prevIsPlayerLooking)
        {
            OnPlayerStoppedLooking(playerLooking);
        }

        prevIsPlayerLooking = isPlayerLooking;
        isPlayerLooking = false;
    }

    public void OnLookedAt(IPlayer player)
    {
        isPlayerLooking = true;

        if(prevIsPlayerLooking)
        {
            onPlayerKeepsLookingAt.Invoke(player);
            return;
        }

        playerLooking = player;
        onPlayerStartedLookingAt.Invoke(player);
    }

    private void OnPlayerStoppedLooking(IPlayer player)
    {
        onPlayerStoppedLookingAt.Invoke(player);
        player = null;
    }
}
