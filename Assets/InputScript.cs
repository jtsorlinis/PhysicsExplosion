﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputScript : MonoBehaviour {

    public static InputScript UI;


    public GameObject ExplodeButton;
    public GameObject PauseButton;
    public Text MassInput;
    public Text PiecesInput;
    public Text ForceInput;
    public Text VelocityInput;
    public Text GravInput;
    public Slider SpeedSlider;

    ExplosionScript expscript;
    GameObject Explosive;
    Vector3 ZoomLevel;
    bool paused = true;

	// Use this for initialization
	void Start () {
        if(UI == null)
        {
            UI = this;
        }
        else
        {
            Destroy(gameObject);
        }
        UI.Explosive = GameObject.FindWithTag("Explosive");
        Camera.main.transform.LookAt(UI.Explosive.transform);
        ZoomLevel = Camera.main.transform.position;
        DontDestroyOnLoad(this.gameObject);
	}

    private void Update()
    {
        if(!paused)
        {
            Time.timeScale = SpeedSlider.value;
        }

        if(MassInput.text != "" && Explosive != null)
        {
			float scale = Mathf.Pow(float.Parse(MassInput.text), 1f / 3f);
			Explosive.transform.localScale = new Vector3(scale, scale, scale);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(ExplodeButton.activeSelf)
            {
                Simulate();
            }
            else
            {
                PausePlay();
            }
        }

        if(Mathf.Abs(Input.mouseScrollDelta.y) > 0 && paused)
        {
            ZoomLevel += Camera.main.transform.forward * Input.mouseScrollDelta.y * 3;
        }
        Camera.main.transform.position = ZoomLevel;

    }

    public void Simulate()
    {
		paused = false;
        UI.expscript = Explosive.GetComponent<ExplosionScript>();
		expscript.Mass = float.Parse(MassInput.text);
		expscript.NoOfPieces = int.Parse(PiecesInput.text);
		expscript.ExplosionForce = float.Parse(ForceInput.text);
        expscript.Velocity.x = float.Parse(VelocityInput.text);
        expscript.gravity = float.Parse(GravInput.text);
        if (VelocityInput.text != "0")
        {
            expscript.ExplosionDelay = 1;
        }
		
        expscript.enabled = true;
        ExplodeButton.SetActive(false);
        PauseButton.SetActive(true);
    }

	public void Reset()
	{
		SceneManager.LoadScene(0);
		ExplodeButton.SetActive(true);
		PauseButton.SetActive(false);
        Time.timeScale = 1;
        paused = true;
	}

    public void PausePlay()
    {
        paused = !paused;
        if(paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
