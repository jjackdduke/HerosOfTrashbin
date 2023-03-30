using Photon.Pun.Demo.Procedural;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Invest : MonoBehaviour
{
    GM gm;
    Random randomObj = new Random();
    int randomValue;
    [SerializeField] int ratio;

    [SerializeField] int Bills;
    [SerializeField] float stock = 20f;
    [SerializeField ]float currentStock;

    [SerializeField] ParticleSystem startInvest;
    [SerializeField] ParticleSystem doneInvest;

    [SerializeField]
    private TextMeshProUGUI actionText;


    BGM bgm;
    [SerializeField] AudioClip StatueSound;
    AudioSource audioSource;
    bool isNear = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GM>();

        bgm = FindObjectOfType<BGM>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = StatueSound;
        audioSource.Play();
        currentStock = stock;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime * 0.4f;
            bgm.AudioSource.volume -= Time.deltaTime * 0.2f;
        }
        if (!isNear && audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime * 0.4f;
            if (bgm.AudioSource.volume < 0.2f)
            {
                bgm.AudioSource.volume += Time.deltaTime * 0.2f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isNear = true;
            InfoAppear(gm.Gold);
            if (gm.Gold >= currentStock)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    DoInvest();                    
                }

            }
            if (Bills > 0)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    GetResult();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isNear = false;
        InfoDisappear();
    }

    private void InfoAppear(int money)
    {
        actionText.gameObject.SetActive(true);
        if (Bills > 0)
        {
            actionText.text = ratio + "% sell now" + (int)(Bills * currentStock) + "Gold" + "<color=yellow>" + "(R)" + "</color>";
            if (money > currentStock)
            {
                actionText.text = ratio + "% sell now" + (int)(Bills * currentStock) + "Gold" + "<color=yellow>" + "(R)" + "</color>" + 
                    "more money, more happy" + "Invest " + currentStock + "Gold more" + "<color=yellow>" + "(E)" + "</color>";
            }
        }
        else
        {
            if (Bills == 0)
            {
                actionText.text = "SSJJ stock is at a bargain right now!" + "Invest " + currentStock + "Gold" + ratio + "%" + "<color=yellow>" + "(E)" + "</color>";
            }
            else if (money > currentStock)
            {
                actionText.text = "more money, more happy" + "Invest " + currentStock + "Gold more" + "<color=yellow>" + "(E)" + "</color>";
            }
            else
            {
                actionText.text = "you don up sir.";
            }
        }
        
    }

    private void InfoDisappear()
    {
        actionText.gameObject.SetActive(false);
    }

    void DoInvest()
    {
        gm.Withdraw((int)currentStock, false);
        Bills += 1;
        Instantiate(startInvest, transform.position, Quaternion.identity);
    }

    public void ThisWave (int Wave)
    {
        Debug.Log("wave" + Wave);
        Result();
    }

    void Result()
    {
        randomValue = randomObj.Next(0, 46);
        ratio = randomValue - 15;
        currentStock = (int)(currentStock * (1 + ((float)ratio / 100)));
    }

    void GetResult()
    {
        gm.Withdraw((int)(Bills * currentStock), true);
        Bills = 0;
        Instantiate(doneInvest, transform.position, Quaternion.identity);
    }
}
