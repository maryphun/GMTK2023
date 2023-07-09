using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public enum NoteType
{
    Blue_Left,
    Blue_Right,
    Red
};

public class TrackManager : MonoBehaviour
{
    [SerializeField] Sprite rednote;
    [SerializeField] Sprite bluenote_left;
    [SerializeField] Sprite bluenote_right;

    [Header("Debug")]
    [SerializeField] float timeElapsed = 0.0f;
    [SerializeField] int totalNoteCount = 0;
    [SerializeField] int spawnedNote = 0;

    [Header("References")]
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    [SerializeField] private DrumsticksController drumsticks;
    [SerializeField] private SceneCtrl sceneController;

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
        totalNoteCount = noteList.Count;
        spawnedNote = 0;
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
                    SpawnNote(note, spawnedNote);
                    noteList.RemoveAt(i);
                    i--;
                }
            }
        }

        // end

        yield return new WaitForSeconds(7.0f);

        sceneController.SetAlpha(1.0f, 4.0f);
        AudioManager.Instance.StopMusicWithFade(3.0f);

        yield return new WaitForSeconds(5.0f);

        AudioManager.Instance.DestroyCurrentMusicSource();
        drumsticks.SubmitResult();

        SceneManager.LoadScene(2);
    }

    private void SpawnNote(Note detail, int noteIndex)
    {
        GameObject objToSpawn;
        //spawn object
        objToSpawn = new GameObject("note [" + detail.beatTiming.ToString() + "]");
        //Add Components
        var sprite = objToSpawn.AddComponent<SpriteRenderer>();

        Sprite targetSprite = rednote;
        if (detail.noteType == NoteType.Blue_Left)
        {
            targetSprite = bluenote_left;
        }
        else if (detail.noteType == NoteType.Blue_Right)
        {
            targetSprite = bluenote_right;
            sprite.flipX = true;
        }
        else if (detail.noteType == NoteType.Red)
        {
            targetSprite = rednote;
        }
        sprite.sprite = targetSprite;
        sprite.sortingLayerName = "Notes";
        sprite.sortingOrder = noteIndex;

        var objTransform = objToSpawn.GetComponent<Transform>();
        objTransform.SetParent(this.transform);

        var noteObjScript = objToSpawn.AddComponent<NoteObject>();
        noteObjScript.Initialize(detail.travelTime, drumsticks, startPosition.localPosition, endPosition.localPosition, objTransform, detail.noteType);
    }

    public Transform GetEndPosition()
    {
        return endPosition;
    }

    public int GetTotalNoteCount()
    {
        return totalNoteCount;
    }
}
