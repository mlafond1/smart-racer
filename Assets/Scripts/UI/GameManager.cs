using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    private string gamemode = "";
    private int piste;

    public string GetGameMode()
    {
        return this.gamemode;
    }

    public void SetGameMode(string _gamemode)
    {
        this.gamemode = _gamemode;
    }

    public int GetPiste()
    {
        return this.piste;
    }

    public void SetPiste(int id)
    {
        this.piste = id;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
