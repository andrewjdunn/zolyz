using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public delegate void PlayerStateChanged(PlayerStateData stateData);
    public event PlayerStateChanged OnPlayerStateChanged;

    [Serializable]
    public class PlayerStateData
    {
        [SerializeField]
        internal int livesLeft;
    }

    [SerializeField]
    public PlayerStateData activePlayerState;

    private static PlayerState _instance;
    private readonly int START_LEVES = 3;

    void Awake()
    {
        if(_instance == null )
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadData();
    }

    public void LoseALife()
    {
        --activePlayerState.livesLeft;
        OnPlayerStateChanged?.Invoke(activePlayerState);
    }

    private void LoadData()
    {
        activePlayerState.livesLeft = START_LEVES;
    }
}
