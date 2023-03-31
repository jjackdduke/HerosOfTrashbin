using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = System.Random;

public class Statue : MonoBehaviour
{
    Random randomObj = new Random();
    Enemy[] TargetEnemies;
    [SerializeField] ParticleSystem punish;
    [SerializeField] ParticleSystem punishEffect;
    [SerializeField] ParticleSystem deBuffEffect;
    public bool punishable = true;
    public bool Punishable { get { return punishable; } set { punishable = value; } }

    [SerializeField]
    private TextMeshProUGUI actionText;

    int randomValue;
    [SerializeField] float startPercent = 50;
    float currentPercent;

    public GameObject StatueParticle;

    BGM bgm;
    GM gm;
    bool isNear = false;

    // Start is called before the first frame update
    void Start()
    {
        StatueParticle = transform.GetChild(1).gameObject;
        StatueParticle.SetActive(punishable);

        bgm = FindObjectOfType<BGM>();
        gm = FindObjectOfType<GM>();

        currentPercent = startPercent;
    }

    // Update is called once per frame
    

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        isNear = true;
    //        InfoAppear(punishable);
    //        if (punishable)
    //        {
    //            if (Input.GetKey(KeyCode.E))
    //            {
    //                Pray();
    //                punishable = false;
    //                StatueParticle.SetActive(punishable);
    //            }

    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    isNear = false;
    //    InfoDisappear();
    //}

    //private void InfoAppear(bool show)
    //{
    //    actionText.gameObject.SetActive(true);
    //    if (show)
    //    {
    //        actionText.text = "The Jangseung is glaring. " + "Pray" + currentPercent + "%" + "<color=yellow>" + "(E)" + "</color>";
    //    }
    //    else
    //    {
    //        actionText.text = "The glare of the Jangseung has disappeared.";   
    //    }
    //}

    //private void InfoDisappear()
    //{
    //    actionText.gameObject.SetActive(false);
    //}

    public void Pray()
    {
        randomValue  = randomObj.Next(0, 101);
        GameObject[] Sounds;
        Sounds = GameObject.FindGameObjectsWithTag("BackgroundSound");
        List<AudioSource> allAudio = new List<AudioSource>();
        foreach (GameObject sound in Sounds)
        {
            allAudio.Add(sound.GetComponent<AudioSource>());
        }
        GameObject.FindGameObjectsWithTag("Enemy");
        foreach (AudioSource audio in allAudio)
        {
            audio.mute = true;
        }
        bool sucees = randomValue < currentPercent;
        ParticleSystem nuke = Instantiate(punish, gm.EnemyNavi.transform.position, Quaternion.identity);
        if (!sucees)
        {
            foreach (ParticleSystem nukeChild in nuke.GetComponentsInChildren<ParticleSystem>())
            {
                var em = nukeChild.emission;
                em.enabled = false; 
            }
        }
        // all enemies Die!
        StartCoroutine(doPunish(sucees));
        currentPercent = currentPercent / 2;
        StartCoroutine(playAgain(allAudio));

    }

    IEnumerator doPunish(bool sucees)
    {
        yield return new WaitForSeconds(7f);
        GameObject TargetSource = GameObject.Find("EnemyPath");
        TargetEnemies = TargetSource.GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in TargetEnemies)
        {
            if (sucees)
            {
                enemy.ProcessHit(Mathf.Infinity, punishEffect);
            }
            else
            {
                doom(enemy);                
            }
        }
        yield break;
    }

    void doom(Enemy thisEnemy)
    {
        StartCoroutine((doomEnemy(thisEnemy)));
    }


    IEnumerator doomEnemy(Enemy enemy)
    {
        int cnt = 0;
        while (cnt < 4)
        {
            enemy.ProcessReduceSpeed(-3, deBuffEffect);
            enemy.ProcessReduceArmor(-3, deBuffEffect);
            cnt++;
            yield return new WaitForSeconds(4f);
        }
        
    }

    IEnumerator playAgain(List<AudioSource> play)
    {
        yield return new WaitForSeconds(14f);
        foreach (AudioSource audio in play)
        {
            audio.mute = false;
        }
        yield break;
    }
}
