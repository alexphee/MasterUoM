using UnityEngine;

public class URLopener : MonoBehaviour
{
    public string Url;

    public void Open() {
        Application.OpenURL(Url);
    }
    
}
