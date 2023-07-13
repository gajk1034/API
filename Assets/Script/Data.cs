using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data 
{
    public string amiiboSeries;
    public string name;
    public string image;
}

[System.Serializable]
public class AmiiboResponse
{
    public List<Data> amiibo;
}
