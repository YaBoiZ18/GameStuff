using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] private string textIntro; //sound or music
    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }
    // Update is called once per frame
    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100;
        txt.text = textIntro + volumeValue.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
