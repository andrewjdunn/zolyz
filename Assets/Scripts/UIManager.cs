using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private GameObject playerStateObject;

    private PlayerState playerState;
    void Start()
    {
        playerState = playerStateObject.GetComponent<PlayerState>();
        playerState.OnPlayerStateChanged += PlayerState_OnPlayerStateChanged;
        // TODO: Register and recieve.. shortcut for this?
        PlayerState_OnPlayerStateChanged(playerState.activePlayerState);
    }

    private void PlayerState_OnPlayerStateChanged(PlayerState.PlayerStateData stateData)
    {
        // TODO: iln8
        livesText.text = $"Lives: {stateData.livesLeft}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
