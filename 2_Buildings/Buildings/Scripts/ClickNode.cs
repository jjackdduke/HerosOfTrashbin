using System.Collections;
using UnityEngine;
using static TowerTemplate;
using Random = System.Random;
using TMPro;

public class ClickNode : MonoBehaviour
{
    // ?좎룞?숉솢?좎룞?숉솕 ?좎룞?쇿뜝?숈삕
    public Color StartColor;

    // ?쒎뜝?숈삕???좎룞?쇿뜝?숈삕
    public Color SelectColor;

    // ?좎뙏?먯삕 ?좎룞?숈튂?좎룞???좎룞?숈튂?좎떎?듭삕?좎뙇?먯삕?좎룞???좎룞?쇿뜝?숈삕
    [SerializeField] int isBuilt = 0;

    // ?좎룞?숈튂?좎룞????좎룞?쇿뜝?숈삕?좎룞?쇿뜝?숈삕 ?좎룞?쇿뜝?쒕챿??
    [SerializeField] BuildingTemplate Building;
    [SerializeField] TowerTemplate[] TowerPrefabs;


    [SerializeField]
    private TextMeshProUGUI actionText;

    // ?좎룞?쇿뜝?숈삕?좎룞?쇿뜝?숈삕 ?좎룞?쇿뜝?숈삕
    public Renderer rend;

    // ?좎룞?쇿뜝?숈삕?좎룞???좎룞???좎룞?쇿뜝?숈삕 ?좎룞?쇿뜝?숈삕
    Random randomObj = new Random();
    int randomValue;

    GM gm;

    // ?좎떎?쎌삕?좎룞?쇿뜝?숈삕?좎룞??
    private float buildDelay = 0.3f;
    GameObject arrow;

    bool buildActivated;

    void Awake()
    {
        gm = FindObjectOfType<GM>();
        rend = GetComponent<Renderer>();
        actionText = GameObject.Find("ShowText").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        rend.material.color = StartColor;
        arrow = transform.GetChild(2).gameObject;
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            BuildInfoAppear();

            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("E clicked");
                BuildTower();
                Debug.Log("tower Builded");
            }

            Debug.Log("?????걗???臾롫젏");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        BuildInfoDisappear();
    }

    public bool CreateTower(TowerTemplate towerTemplate, Vector3 position)
    {
        GameObject projectilePrefab = towerTemplate.weapon[0].Prefab;

        if (gm == null)
        {
            Debug.Log("GM 이 없습니다");
            return false;
        }
        if (gm.TowerCnt > 0) // Tower갯수가 있을때
            {

            GameObject towerClone = Instantiate(projectilePrefab, position, Quaternion.identity);
            Debug.Log("towerClone : " + towerClone);
            gm.BuildTower(); // 타워개수 -1
            StartCoroutine(Build());
            // cost += inflation;

            towerClone.GetComponent<TowerWeapon>().SetUp(towerTemplate, actionText, Building);
            gameObject.SetActive(false);

            return true;
        }
        
        return false;
    }

    IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);

            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(true);
            }
        }

    }


    private void BuildTower()
    {
        if (isBuilt == 0 && buildActivated)
        {
            bool isSuccessful;
            randomValue = randomObj.Next(0, 6);
            Debug.Log(randomValue);
            rend.material.color = SelectColor;
            isSuccessful = CreateTower(TowerPrefabs[randomValue], transform.position);
            isBuilt = randomValue;
            BuildInfoDisappear();
        }
    }


    private void BuildInfoAppear()
    {
        buildActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text =  "Build Tower" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void BuildInfoDisappear()
    {
        buildActivated = false;
        actionText.gameObject.SetActive(false);
    }

    public void Arrow(bool show)
    {
        arrow.SetActive(show);
    }
}
