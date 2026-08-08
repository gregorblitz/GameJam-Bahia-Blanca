using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerStatsUI : MonoBehaviour
{
    [System.Serializable]
    public class StatScrollbar
    {
        public StatType stat;
        public Scrollbar scrollbar;
    }

    [Header("Customer Queue")]
    [SerializeField] private CustomerQueue customerQueue;

    [Header("Stat Scrollbars")]
    [SerializeField] private List<StatScrollbar> scrollbarList = new();

    private Dictionary<StatType, Scrollbar> scrollbars = new();

    private void Awake()
    {
        BuildDictionary();
    }

    private void Update()
    {
        UpdateStatsUI();
    }

    private void BuildDictionary()
    {
        scrollbars.Clear();

        foreach (StatScrollbar entry in scrollbarList)
        {
            if (entry.scrollbar == null)
                continue;

            scrollbars[entry.stat] = entry.scrollbar;
        }
    }

    private void UpdateStatsUI()
    {
        if (customerQueue == null)
            return;

        Customer currentCustomer = customerQueue.CurrentCustomer;

        if (currentCustomer == null)
        {
            ClearScrollbars();
            return;
        }

        CharacterStats stats = currentCustomer.GetComponent<CharacterStats>();

        if (stats == null)
        {
            ClearScrollbars();
            return;
        }

        foreach (KeyValuePair<StatType, Scrollbar> entry in scrollbars)
        {
            float value = stats.GetStat(entry.Key);

            entry.Value.value = value / 100f;
        }
    }

    private void ClearScrollbars()
    {
        foreach (Scrollbar scrollbar in scrollbars.Values)
        {
            if (scrollbar != null)
                scrollbar.value = 0f;
        }
    }

    public void AssignScrollbar(StatType stat, Scrollbar scrollbar)
    {
        if (scrollbar == null)
            return;

        scrollbars[stat] = scrollbar;
    }

    public void RemoveScrollbar(StatType stat)
    {
        scrollbars.Remove(stat);
    }
}