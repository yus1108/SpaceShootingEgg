using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour {
    public float TOP_BOUNDARY;
    public float BTM_BOUNDARY;
    public float Left_BOUNDARY;
    public float Right_BOUNDARY;
    public float uiBaseScreenHeight = 768f;

    public int difficulty;
    public long score;
    public bool isCooldown;

    [HideInInspector]
    public GameObject motherPlanet;
    [HideInInspector]
    public GameObject[] moons;
    [HideInInspector]
    public Camera myCam;

    public GameObject motherPlanetPrefab;
    public GameObject[] moonsPrefab;
    public GameObject[] cometsPrefab;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        motherPlanet = null;
        isCooldown = false;
        score = 0;
        myCam = GameObject.Find("Main Camera").GetComponent<Camera>();

        Vector3 instPos = myCam.ScreenToWorldPoint(new Vector3(0, 0, myCam.transform.position.z * -1));
        Left_BOUNDARY = instPos.x;
        BTM_BOUNDARY = instPos.y;
        instPos = myCam.ScreenToWorldPoint(new Vector3(myCam.pixelWidth, myCam.pixelHeight, myCam.transform.position.z * -1));
        Right_BOUNDARY = instPos.x;
        TOP_BOUNDARY = instPos.y * 0.3f;
        // first comet
        float ypos = Random.Range(0, Mathf.Abs(BTM_BOUNDARY - TOP_BOUNDARY)) + TOP_BOUNDARY;
        float xpos = Random.Range(0, Mathf.Abs(Right_BOUNDARY - Left_BOUNDARY)) + Left_BOUNDARY;
        int typeOfComet = Random.Range(0, cometsPrefab.Length);
        Instantiate(cometsPrefab[typeOfComet], new Vector3(xpos, ypos, 0), Quaternion.identity);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 instPos = myCam.ScreenToWorldPoint(new Vector3(myCam.pixelWidth, myCam.pixelHeight, myCam.transform.position.z * -1));
        Right_BOUNDARY = instPos.x;
        TOP_BOUNDARY = instPos.y;
        instPos = myCam.ScreenToWorldPoint(new Vector3(0, 0, myCam.transform.position.z * -1));
        Left_BOUNDARY = instPos.x;
        BTM_BOUNDARY = instPos.y * 0.3f;
        

        StartCoroutine("CreateComet");
        if (motherPlanet == null || motherPlanet.transform.position.x < Left_BOUNDARY)
        {
            float ypos = Random.Range(0, Mathf.Abs(BTM_BOUNDARY - TOP_BOUNDARY)) + TOP_BOUNDARY;
            float xpos = Random.Range(0, Mathf.Abs(Right_BOUNDARY - Left_BOUNDARY)) + Left_BOUNDARY;
            motherPlanet = Instantiate(motherPlanetPrefab, new Vector3(xpos, ypos, 0), Quaternion.identity);
            motherPlanet.GetComponent<MovingObject>().initAccel = Random.Range(-200f, -50f);

            int numMoons = Random.Range(0, 2) + difficulty;
            moons = new GameObject[numMoons];
            for (int i = 0; i < numMoons; i++)
            {
                ypos = Random.Range(motherPlanet.transform.position.y + 1, motherPlanet.transform.position.y + 50);
                xpos = Random.Range(motherPlanet.transform.position.x + 1, motherPlanet.transform.position.x);
                int typeOfMoon = Random.Range(0, moonsPrefab.Length);
                moons[i] = Instantiate(moonsPrefab[typeOfMoon], new Vector3(xpos, ypos, 0), Quaternion.identity);
                moons[i].GetComponent<MoonObject>().motherPlanet = motherPlanet;
            }
            difficulty++;
        }
	}

    IEnumerator CreateComet()
    {
        if (!isCooldown)
        {
            isCooldown = true;
            int numComets = Random.Range(0, 2) * difficulty;
            for (int i = 0; i < numComets; i++)
            {
                float ypos = Random.Range(0, Mathf.Abs(BTM_BOUNDARY - TOP_BOUNDARY)) + TOP_BOUNDARY;
                float xpos = Random.Range(0, Mathf.Abs(Right_BOUNDARY - Left_BOUNDARY)) + Left_BOUNDARY;
                int typeOfComet = Random.Range(0, cometsPrefab.Length);
                Instantiate(cometsPrefab[typeOfComet], new Vector3(xpos, ypos, 0), Quaternion.identity);
            }
            yield return new WaitForSeconds(5f);
            isCooldown = false;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = GUI.skin.GetStyle("Button");
        GUIStyle style2 = GUI.skin.GetStyle("Box");
        style.fontSize = GetScaledFontSize(24);
        style2.fontSize = GetScaledFontSize(24);

        if (FindObjectOfType<PlayerController>() != null)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height * 0.1f), "Level: " + difficulty + "\nScore: " + score);
            if (FindObjectOfType<PlayerController>().isGameOver)
            {
                Time.timeScale = 0;
                GUI.Box(new Rect(Screen.width / 2 - Screen.width * 0.1f, Screen.height / 2 - Screen.height * 0.15f,
                    2 * Screen.width / 10, 2 * Screen.height * 0.12f), "Game Over!");
                if (GUI.Button(new Rect(Screen.width / 2 - Screen.width * 0.09f, Screen.height / 2 - Screen.height * 0.08f,
                    2 * Screen.width * 0.09f, 2 * Screen.height / 30), "Start Again"))
                {
                    SceneManager.LoadScene("Level1");
                }
                if (GUI.Button(new Rect(Screen.width / 2 - Screen.width * 0.09f, Screen.height / 2,
                    2 * Screen.width * 0.09f, 2 * Screen.height / 30), "Exit"))
                    SceneManager.LoadScene("Intro");
            }
        } else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - Screen.width * 0.09f, Screen.height / 2 - Screen.height * 0.04f,
                    2 * Screen.width * 0.09f, 2 * Screen.height * 0.04f), "Start"))
            {
                SceneManager.LoadScene("Level1");
            }
        }
        
    }

    private int GetScaledFontSize(int baseFontSize)
    {
        float uiScale = Screen.height / uiBaseScreenHeight;
        int scaledFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
        return scaledFontSize;
    }


}
