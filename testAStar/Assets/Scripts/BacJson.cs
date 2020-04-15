using UnityEngine;
using System.Collections;
using System.Text;

public class BacJson
{
    public static string ToJson<T>(DataSave<T> gr)
    {
        StringBuilder resultsJson = new StringBuilder();

        for (int i = 0; i < gr.results.Count; i++)
        {
            resultsJson.Append(JsonUtility.ToJson(gr.results[i]));
            if (i < gr.results.Count - 1)
            {
                resultsJson.Append("|");
            }
        }
        // resultsJson += "]}";
        return resultsJson.ToString();
    }
    public static DataSave<T> FromJson<T, T1, T2>(string jsonOut)
    {
        //jsonOut.('[');
       // jsonOut.Remove(']');
        jsonOut.Replace("},", "}|");
        string[] s = jsonOut.Split('|');
        DataSave<T> game = new DataSave<T>();
        for (int i = 0; i < s.Length; i++)
        {
            T sd = JsonUtility.FromJson<T>(s[i]);
            var _temp = sd.GetType().GetField("NAME").GetValue(sd);
           // Debug.Log(_temp+"-"+ typeof(T2).ToString());
            if (_temp.ToString().Equals(typeof(T1).ToString()))
            {
                sd = (T)(object)JsonUtility.FromJson<T1>(s[i]);
            }
            else if (_temp.ToString().Equals(typeof(T2).ToString()))
            {
                sd = (T)(object)JsonUtility.FromJson<T2>(s[i]);
              //  Debug.Log(sd);
            }
            else
            {
               // Debug.Log(sd);
            }
            game.results.Add(sd);
        }
        return game;
    }
    public static DataSave<T> FromJson<T, T1, T2, T3>(string jsonOut)
    {
        //jsonOut.('[');
        // jsonOut.Remove(']');
        jsonOut.Replace("},", "}|");
        string[] s = jsonOut.Split('|');
        DataSave<T> game = new DataSave<T>();
        for (int i = 0; i < s.Length; i++)
        {
            T sd = JsonUtility.FromJson<T>(s[i]);
            var _temp = sd.GetType().GetField("NAME").GetValue(sd);
            if (_temp.ToString().Equals(typeof(T1).ToString()))
            {
                sd = (T)(object)JsonUtility.FromJson<T1>(s[i]);
            }
            else if (_temp.ToString().Equals(typeof(T2).ToString()))
            {
                sd = (T)(object)JsonUtility.FromJson<T2>(s[i]);
            }
            else if (_temp.ToString().Equals(typeof(T3).ToString()))
            {
                sd = (T)(object)JsonUtility.FromJson<T3>(s[i]);
            }
            game.results.Add(sd);
        }
        return game;
    }
}
