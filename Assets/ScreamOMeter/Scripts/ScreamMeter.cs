using UnityEngine;

public class ScreamMeter : MonoBehaviour
{
    public Gradient color;
    public float speed;
    public UnityEngine.UI.Image bar;

    public void Update() 
    {
        if (Input.GetKey(KeyCode.UpArrow))
            bar.fillAmount += speed * Time.deltaTime;
        else bar.fillAmount -= speed * Time.deltaTime;

        bar.color = color.Evaluate(bar.fillAmount);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
