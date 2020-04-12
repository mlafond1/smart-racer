using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    private string gamemode = "";
    private int piste;

    public string getGameMode()
    {
        return this.gamemode;
    }

    public void setGameMode(string _gamemode)
    {
        this.gamemode = _gamemode;
    }

    public int getPiste()
    {
        return this.piste;
    }

    public void setPiste(int id)
    {
        this.piste = id;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
