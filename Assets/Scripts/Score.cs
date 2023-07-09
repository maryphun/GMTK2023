using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : SingletonMonoBehaviour<Score>
{
    int _combo, _hit, _miss,_score;
    public void SetResult(int combo, int hit, int miss, int score)
    {
        _combo = combo;
        _hit = hit;
        _miss = miss;
        _score = score;
    }

    public int GetCombo()
    {
        return _combo;
    }

    public int GetHit()
    {
        return _hit;
    }

    public int GetMiss()
    {
        return _miss;
    }

    public int GetScore()
    {
        return _score;
    }
}
