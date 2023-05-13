using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteProperty : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool canChangeSound;
    private NotesManager notesMana;

    private Vector3 randomPos;
    private Vector3 targetPos;
    private float randomRotate;
    private AudioSource noteAuSource;
    private SpriteRenderer spriteRen;

    // Start is called before the first frame update
    void Start()
    {
        notesMana = GameObject.FindWithTag("NotesManager").GetComponent<NotesManager>();
        noteAuSource = GetComponent<AudioSource>();
        spriteRen = GetComponent<SpriteRenderer>();

        if (notesMana.pushedNoteNum < notesMana.noteAuClips.Length - 1)
        {
            noteAuSource.clip = notesMana.noteAuClips[notesMana.pushedNoteNum + 1];
        }
        else
        {
            noteAuSource.clip = notesMana.noteAuClips[0];
        }

        //��]���x�A�i�s�ʒu������
        randomPos = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
        randomRotate = Random.Range(-0.5f, 0.5f);
        targetPos = randomPos - new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0) * 2;
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ��̎��s
        this.gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        this.gameObject.transform.Rotate(0, 0, randomRotate, Space.Self);
    }

    public void ClickNote0()
    {
        canChangeSound = true;
        noteAuSource.PlayOneShot(notesMana.noteAuClips[notesMana.pushedNoteNum]);
    }

    public void ClickNote1()
    {
        noteAuSource.PlayOneShot(noteAuSource.clip);
        if (notesMana.isInterval)
        {
            return;
        }
        else
        {
            notesMana.isInterval = true;
            canChangeSound = true;
            StartCoroutine(SetNoteChangeInterval());
        }
    }

    private IEnumerator SetNoteChangeInterval()
    {
        ++notesMana.pushedNoteNum;

        //�ō����ɒB�����珉����
        if (notesMana.pushedNoteNum == 7)
        {
            notesMana.pushedNoteNum = 0;
        }

        /*yield return new WaitForSeconds (9f);
        notesMana.isInterval = false;*/
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyNoteCollider"))
        {
            if (!canChangeSound)
            {
                notesMana.pushedNoteNum = 0;
            }

            if (this.gameObject.CompareTag("Note1"))
            {
                notesMana.isInterval = false;
            }
            spriteRen.DOFade(0, 0.5f);
            Invoke("DestroyNote", 1f);
        }
    }

    private void DestroyNote()
    {
        Destroy(this.gameObject);
    }
}