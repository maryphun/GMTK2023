using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : SingletonMonoBehaviour<Score>
{
    [SerializeField] int _combo, _hit, _miss,_score;
    [SerializeField] int _maxNoteCnt;
    [SerializeField] bool _isScoreSubmitted = false;
    public void SetResult(int combo, int hit, int miss, int score)
    {
        _combo = combo;
        _hit = hit;
        _miss = miss;
        _score = score;
        _isScoreSubmitted = true;

        DontDestroyOnLoad(gameObject);
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

    public void SetMaxNoteCnt(int value)
    {
        _maxNoteCnt = value;
    }

    public int GetMaxNoteCnt()
    {
        return _maxNoteCnt;
    }

    public bool IsScoreSubmitted()
    {
        return _isScoreSubmitted;
    }
}
