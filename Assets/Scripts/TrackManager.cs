using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum NoteType
{
    Blue,
    Red
};

public class TrackManager : MonoBehaviour
{
    [SerializeField] Sprite rednote;
    [SerializeField] Sprite bluenote;

    [Header("Debug")]
    [SerializeField] float timeElapsed = 0.0f;

    [Header("References")]
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    [SerializeField] private DrumsticksController drumsticks;

    [System.Serializable]
    public struct Note
    {
        public NoteType noteType;
        public float travelTime;
        public float beatTiming;
    }

    [SerializeField] List<Note> noteList = new List<Note>();

    public void StartTrack()
    {
        StartCoroutine(Track());
    }

    IEnumerator Track()
    {
        timeElapsed = 0.0f;
        while (noteList.Count >= 1)
        {
            yield return new WaitForFixedUpdate();
            timeElapsed += Time.fixedDeltaTime;
            for (int i = 0; i < noteList.Count; i++)
            {
                Note note = noteList[i];
                float spawnTiming = (note.beatTiming - note.travelTime);
                if (spawnTiming < 0.0f)
                {
                    Debug.Log("NOTE TIMING IS NEGATIVE");
                }
                else if (spawnTiming <= timeElapsed)
                {
                    SpawnNote(note);
                    noteList.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    private void SpawnNote(Note detail)
    {
        GameObject objToSpawn;
        //spawn object
        objToSpawn = new GameObject("note [" + detail.beatTiming.ToString() + "]");
        //Add Components
        var sprite = objToSpawn.AddComponent<SpriteRenderer>();

        Sprite targetSprite = detail.noteType == NoteType.Blue ? bluenote : rednote;
        sprite.sprite = targetSprite;
        sprite.sortingLayerName = "Notes";
        sprite.sortingOrder = 0;

        var objTransform = objToSpawn.GetComponent<Transform>();
        objTransform.SetParent(this.transform);

        var noteObjScript = objToSpawn.AddComponent<NoteObject>();
        noteObjScript.Initialize(detail.travelTime, drumsticks, startPosition.localPosition, endPosition.localPosition, objTransform, detail.noteType);
    }
}
