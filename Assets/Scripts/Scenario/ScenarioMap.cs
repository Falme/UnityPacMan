﻿using System.Collections.Generic;

public class ScenarioMap
{
    private string scenarioString;

    public string GetScenarioString()
    {
        if (scenarioString == null) scenarioString = string.Empty;
        return scenarioString;
    }

    public List<List<char>> GetScenarioGrid(int width, int height)
    {
        if (GetScenarioString().Equals(string.Empty)) return new List<List<char>>();

        List<List<char>> _map = new List<List<char>>();
        int index = 0;
        string _scenarioString = GetScenarioString();

        for (int h = 0; h < height; h++)
        {
            _map.Add(new List<char>());

            for (int w = 0; w < width; w++, index++)
            {
                if (_scenarioString.Length <= index)
                {
                    _map[h].Add('0');
                    continue;
                }
                _map[h].Add(_scenarioString[index]);
            }
        }

        return _map;
    }

    public void SetScenarioString(string scenario)
    {
        scenarioString = scenario;
    }

    public float GetCenteredTilePositionByIndex(int length, int index, float distance)
    {
        if (length <= 0 || index < 0 || index >= length) 
            return 0f;

        float result = ((-((length - 1f) / 2f))*distance)+ (index* distance);
        return result;
    }
}
