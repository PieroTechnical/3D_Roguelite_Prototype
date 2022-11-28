using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] RawImage fullscreenMap;
    [SerializeField] Transform mapParent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

        if (Input.GetKeyDown(KeyCode.Z)) Application.LoadLevel(0);

        if (Input.GetKey(KeyCode.M))
        {

            mapParent.gameObject.SetActive(true);
            fullscreenMap.texture = World.Gen.LevelController.ctrl.dungeonTexture;

        }

        else mapParent.gameObject.SetActive(false);

    }
}
