using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChosenUI : MonoBehaviour
{
    Player player;
    public RectTransform[] weapons;

    private float currentY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        currentY = weapons[0].anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < 2; i ++)
        {
            if (i == player.weaponChosen)
            {
                weapons[player.weaponChosen].anchoredPosition = new Vector3(weapons[player.weaponChosen].anchoredPosition.x, currentY+ 5,0);
            } else
            {
                weapons[i].anchoredPosition = new Vector3(weapons[i].anchoredPosition.x, currentY,0);
            }



        }


       
    }
}
