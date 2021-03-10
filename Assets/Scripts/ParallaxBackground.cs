using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float startPosX, startPosY, length;
    public Transform camera;
    public float paralaxEffect;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float tempX = camera.position.x * (1 - paralaxEffect);
        float tempY = camera.position.y * (1 - paralaxEffect);
        float distX = camera.position.x * paralaxEffect;
        float distY = camera.position.y * paralaxEffect;

        // двигаем фон с поправкой на paralaxEffect
        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        // если камера перескочила спрайт, то меняем startPos
        {
            if (tempX > startPosX + length)
                startPosX += length;
            else if (tempX < startPosX - length)
                startPosX -= length;
        }
        {
            if (tempY > startPosY + length)
                startPosY += length;
            else if (tempY < startPosY - length)
                startPosY -= length;
        }
    }
}
