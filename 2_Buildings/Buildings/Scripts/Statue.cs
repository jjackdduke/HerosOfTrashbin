using System.Collections;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Statue : MonoBehaviour
{
    GameObject[] Sounds;
    Random randomObj = new Random();
    Enemy[] TargetEnemies;
    [SerializeField] ParticleSystem punish;
    [SerializeField] ParticleSystem punishEffect;
    [SerializeField] ParticleSystem deBuffEffect;
    
    [SerializeField]
    private TextMeshProUGUI actionText;

    int randomValue;
    [SerializeField] float startPercent = 50;
    float currentPercent;

    public GameObject StatueParticle;

    BGM bgm;
    GameObject BGMObject;
    GM gm;
    bool isNear = false;

    void Start()
    {
        BGMObject = GameObject.Find("BGM");
        bgm = GameObject.Find("BGM").GetComponent<BGM>();
        gm = FindObjectOfType<GM>();
        int punishable = gm.Punishable;
        StatueParticle = transform.GetChild(1).gameObject;
        StatueParticle.SetActive(punishable > 0);

        currentPercent = startPercent;
    }

    public void Pray()
    {
        randomValue  = randomObj.Next(0, 101);

        GameObject.FindGameObjectsWithTag("Enemy");

        BGMObject.GetComponent<AudioSource>().mute = true;
        Debug.Log($"오디오 소스가 뮤트인지? : {BGMObject.GetComponent<AudioSource>().mute}");
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
        playAgain(BGMObject.GetComponent<AudioSource>());


    }

    IEnumerator doPunish(bool sucees)
    {
        yield return new WaitForSeconds(7f);
        GameObject TargetSource = GameObject.Find("startPoint");
        TargetEnemies = TargetSource.GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in TargetEnemies)
        {
            if (sucees)
            {
                enemy.ProcessHit(Mathf.Infinity, punishEffect);
                currentPercent = currentPercent / 2;
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

    IEnumerator playAgain(AudioSource play)
    {
        yield return new WaitForSeconds(14f);

            play.mute = false;

        yield break;
    }
}
