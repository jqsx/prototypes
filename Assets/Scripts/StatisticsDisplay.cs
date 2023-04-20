using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsDisplay : MonoBehaviour
{
    private Dictionary<string, Tracker> Trackers = new Dictionary<string, Tracker>();
    [SerializeField]
    private List<Tracker> Innit = new List<Tracker>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Tracker tracker in Innit) {
            Trackers.Add(tracker.name, tracker);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Tracker tracker in Trackers.Values)
        {
            tracker.Update();
        }
    }

    public void setTrackerValue(string name, float value)
    {
        Trackers.TryGetValue(name, out Tracker tracker);
        if (tracker != null) tracker.setValue(value);
    }

    [System.Serializable]
    public class Tracker
    {
        public string name;
        public Slider slider;
        public float value = 0f;

        public void setValue(float value)
        {
            this.value = value;
        }

        public void Update()
        {
            slider.value = Mathf.Lerp(slider.value, value, Time.deltaTime * 5f);
        }
    }
}