﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : Lines
{
    //publics
    public Kogel kogel;

    public float delayTime = 1f;
    //privates

    //line
    GameObject closestEnemyLine;
    GameObject aimLine;
    //enemy
    Enemy closestEnemy;
    //time
    private float delay;
    void Start()
    {
        this.closestEnemy = this.findClosestEnemy();
        this.closestEnemyLine = this.makeLine(Vector2.up, Color.red);
        this.aimLine = this.makeLine(Vector2.zero, Color.yellow);
        delay = delayTime;
    }

    void FixedUpdate()
    {
        //vind de dichts bij zijnde enemy
        this.closestEnemy = this.findClosestEnemy();
        //teken een lijn naar de closest enemy in rood
        drawLineTo(this.closestEnemyLine, this.transform.position, closestEnemy.transform.position);
        //bereken de projetile trajectory
        Vector2 trajectory = calulateTrajectory();
        if (delay < 0)
        {
            delay = delayTime;
            this.Shoot(trajectory);
        }
        else
        {
            delay -= Time.fixedDeltaTime;
        }
    }

    void Shoot(Vector2 towards)
    {
        //maak nieuwe kogel
        GameObject tempkogel = (GameObject)Instantiate(kogel.gameObject, this.transform.position, Quaternion.identity);
        //reken de hoek uit
        float angle = VectorMath.CartesianToPolar(towards).x;
        //draai de kogel
        tempkogel.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    Vector2 calulateTrajectory()
    {
        //haal de enemey positie op
        Vector2 enemypos = this.closestEnemy.transform.position;
        //reken de kogel snelheid uit
        float kogelSpeed = kogel.Speed * Time.fixedDeltaTime;
        //berekend de afstand van de enemy
        //dit is a*a+b*b=c(dus pythagoras)
        float enemyDist = Vector2.Distance(this.transform.position, enemypos);
        //reken uit hoelang het duurt voor dat de kogel bij de enemy is
        //time=distance/speed
        float travelTime = enemyDist / kogelSpeed;
        //neem de enemy zijn bewegings vector en vermenigvuldig die met de kogel traveltime
        //positie + bewegings vector * travel time
        Vector2 pre = enemypos + this.closestEnemy.bewegingsVector * travelTime;

        //doe de zelfde berekening maar dan met de nieuwe positie
        enemyDist = Vector2.Distance(this.transform.position, pre);
        //reken de nieuwe adstand uit
        travelTime = enemyDist / kogelSpeed;
        //reken de laaste keer de travel time uit en daar uit krijg je de positie waar je heen moet schieten
        pre = enemypos + this.closestEnemy.bewegingsVector * travelTime;

        this.drawLineTo(this.aimLine, this.transform.position, pre);
        return pre;
    }

    Enemy findClosestEnemy()
    {
        //find alle enemy objecten in het spel
        var enemys = GameObject.FindObjectsOfType<Enemy>();
        if (enemys.Length > 0)
        {
            //zet de begin distance op infinity
            float dist = Mathf.Infinity;
            //maak variable aan waar we de enemy gaan opslaan die het dichts bij is.
            Enemy closest = null;
            //loop door de enemys heen
            for (int i = 0; i < enemys.Length; i++)
            {
                //reken de distance uit tussen dit object en de huidige enemy 
                float curDist = Vector2.Distance(enemys[i].transform.position, this.transform.position);
                //is die distance kleiner dan is de enemy dus ook dichter bij.
                if (curDist < dist)
                {
                    //zet de closest enemy
                    closest = enemys[i];
                    //zet de nieuwe distance
                    dist = curDist;
                }
            }
            //loop is klaar en return de enemy
            return closest;
        }
        //geen enemy gevonden
        return null;
    }
}
