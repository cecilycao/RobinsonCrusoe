using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class DiaryManager : MonoBehaviour
{
    private static DiaryManager _instance;
    public static DiaryManager Instance { get { return _instance; } }

    
    public GameObject DiaryPanel;
    public Animator JournalAnimator;
    public GameObject DiaryUI;
    public GameObject previousButton;
    public GameObject nextButton;
    public Image diaryUIImg;
    public Text dateText;
    public Text WeatherText;
    public Text contentTextLeft;
    public Text contentTextRight;

    List<DiaryPage> pageList = new List<DiaryPage>();
    int currentShowingIndex = 0;

    //document current game info
    private float day;
    List<WeatherState> weathers = new List<WeatherState>();
    WeatherState currentWeather;
    List<WritableEvents> events = new List<WritableEvents>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //day
        GameEvents.Sigton.timeSystem
        .Subscribe(_data =>
        {
            day = _data.DayCount;
        });

        //weather
        GameEvents.Sigton.OnRainStart += () =>
        {
            weathers.Add(WeatherState.RAIN);
            currentWeather = WeatherState.RAIN;
        };
        GameEvents.Sigton.OnRainEnd += () =>
        {
            currentWeather = WeatherState.SUNNY;
        };
        GameEvents.Sigton.OnStormStart += () =>
        {
            weathers.Add(WeatherState.STORM);
            currentWeather = WeatherState.STORM;
        };
        GameEvents.Sigton.OnStormEnd += () =>
        {
            currentWeather = WeatherState.SUNNY;
        };

        //events: island created, island destroyed, talk to npc
        GameEvents.Sigton.onIslandCreated += () =>
        {
            Debug.Log("Receive event create island");
            events.Add(WritableEvents.ISLAND_CREATED);
        };
        //todo: island broken
        GameEvents.Sigton.onTheDestroyedIsland += () =>
        {
            Debug.Log("Receive event stand on the destroyed island");
            events.Add(WritableEvents.ISLAND_DESTROYED);
            createNewPage();
        };
        //todo：talk to npc

        DiaryPanel.SetActive(false);
        JournalAnimator.gameObject.SetActive(false);

    }

    //create new page when a day is end(either because tired, time or die)
    public void createNewPage()
    {
        //create a new page with current info
        DiaryPage newPage = new DiaryPage((int)Mathf.Round(day), weathers, currentWeather, events);

        pageList.Add(newPage);
        showDiary();
        if (!DiaryUI.activeSelf)
        {
            DiaryUI.SetActive(true);
        }
        //clear current info for next day
        //day and weather is alreay subscribed sp we don't need to modify it
        events.Clear();
        weathers.Clear();
    }

    //change diary UI img based on day
    public void changeDiaryUIImg()
    {
        //diaryUI.sprite
    }

    public void showDiary()
    {
        AssertExtension.NotNullRun(GameEvents.Sigton.OnDiaryStart, () =>
        {
            GameEvents.Sigton.OnDiaryStart.Invoke();
        });
        
        JournalAnimator.gameObject.SetActive(true);
        JournalAnimator.SetTrigger("OpenJournal");
        
    }

    public void showContentAfterOpen()
    {
        DiaryPanel.SetActive(true);
        currentShowingIndex = pageList.Count - 1;
        if (currentShowingIndex >= 0)
        {
            displayContent(currentShowingIndex);
        }
    }

    void displayContent(int pageIndex)
    {
        if(pageIndex == 0)
        {
            previousButton.SetActive(false);
            if (pageList.Count <= 1)
            {
                nextButton.SetActive(false);
            }
            
        } else if (pageIndex == pageList.Count - 1)
        {
            nextButton.SetActive(false);
            previousButton.SetActive(true);
        } else
        {
            previousButton.SetActive(true);
            nextButton.SetActive(true);
        }
        DiaryPage currentPage = pageList[pageIndex];
        fillPgae(currentPage);
    }

    void fillPgae(DiaryPage currentPage)
    {
        dateText.text = currentPage.getDate();
        contentTextLeft.text = currentPage.getContentLeft();
        contentTextRight.text = currentPage.getContentRight();
        WeatherText.text = currentPage.getWeather();
       
    }

    public void closeDiary()
    {
        DiaryPanel.SetActive(false);
        JournalAnimator.gameObject.SetActive(false);
        AssertExtension.NotNullRun(GameEvents.Sigton.OnDiaryEnd, () =>
        {
            GameEvents.Sigton.OnDiaryEnd.Invoke();
        });
    }


    public void previousPage()
    {
        if(currentShowingIndex > 0)
        {
            currentShowingIndex--;
            displayContent(currentShowingIndex);
        }
        
    }
    
    public void nextPage()
    {
        if(currentShowingIndex < pageList.Count - 1)
        {
            currentShowingIndex++;
            displayContent(currentShowingIndex);
        }
        
    }

}

public class DiaryPage
{
    public int day;
    WeatherState weather;
    List<WritableEvents> events;

    public DiaryPage(int day, List<WeatherState> weathers, WeatherState currentWeather, List<WritableEvents> events)
    {
        this.day = day;
        this.events = events;
        if (weathers.Contains(WeatherState.STORM) || currentWeather == WeatherState.STORM)
        {
            weather = WeatherState.STORM;
        }
        else if (weathers.Contains(WeatherState.RAIN) || currentWeather == WeatherState.RAIN)
        {
            weather = WeatherState.RAIN;
        } else
        {
            weather = WeatherState.SUNNY;
        }
        
    }

    public string getDate()
    {
        return "Day: " + day + "";
    }

    public string getWeather()
    {
        return weather.ToString();
    }

    public string getFixedContentHead()
    {
        return "FixContent Day" + day;
    }

    public string getFixedContentFoot()
    {
        return "FixContent Day" + day;
    }

    public string getEventContent()
    {
        string eventContent = "";
        if (events.Count == 0)
        {
            return eventContent;
        }
        foreach(WritableEvents e in events)
        {
            eventContent += e.ToString();
        }
           
        return eventContent;
    }

    public string getContentLeft()
    {
        return getFixedContentHead();
    }

    public string getContentRight()
    {
        return getFixedContentFoot();
    }
}

public enum WritableEvents
{
    ISLAND_DESTROYED,
    ISLAND_CREATED,
    TALK_TO_NPC_SATISFIED_0,
    TALK_TO_NPC_SATISFIED_1,
    TALK_TO_NPC_SATISFIED_2
}
