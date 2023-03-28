using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    // �ݾƾ� �� �˾� ������Ʈ�� �������� �Ҵ��Ѵ�. (�����ո��� ������Ʈ ���� ������ �ٸ�)
    [SerializeField] private GameObject popUp;
    public HandlePopUps handlePopUps;

    public void Start()
    {
        handlePopUps = FindObjectOfType<HandlePopUps>();
        GetComponent<Button>().onClick.AddListener(Close);
    }

    public void Close()
    {
        handlePopUps.ClosePopUp(popUp);
    }
}
