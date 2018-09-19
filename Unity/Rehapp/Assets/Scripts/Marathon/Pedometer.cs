using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Pedometer : MonoBehaviour {
    bool cap = false;       //Capturing accelerometer?
    public bool capKernell = false;    //Capturing for the first time?
    float kernellTime = 0;
    public float oneStep = 3.0f;
    public float initDelay = 2.0f;
    public float initStepCount = 3;
    public int kernellDistribution = 2;
    public float presision = 0.79f;
    int stepRestriction = 5; //Para que no cuente pasos en menos de medio segundo
    public int steps = 3;
    float xAcc;
    float yAcc;
    float zAcc;
    Vector3 accSelected;
    public bool greenLight = false;

    //List<float> xKernellData = new List<float>();
    float[] xKernellData = { };
    float[] yKernellData = { };
    float[] zKernellData = { };
    float[] kernell = new float[6];
    float[] realTimeData = {0,0,0,0,0,0};

    private void Update()
    {
        if (kernellTime < (oneStep * initStepCount) && cap && greenLight)
            kernellTime += Time.deltaTime;
    }

    public void StartCapturing()
    {
        if (!cap)
        {
            kernellTime = 0.0f;
            cap = true;
            capKernell = true;
            stepRestriction = 0;
            InvokeRepeating("MarchCaptureAcc", initDelay, 0.1f);
            initDelay = 2;
        }
        else
        {
            CancelInvoke();
            xKernellData = new float[0];
            yKernellData = new float[0];
            zKernellData = new float[0];
            cap = false;
        }
    }

    private void CaptureKernell(float[] data)
    {
        //Quitar señal DC
        float aver = data.Average();
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i] - aver;
        }
        //Detecta el mayor pico y lo pone como el centro del kernell
        //Crea el kernell
        int kernellCenter;
        if (data.Max() > Math.Abs(data.Min())) kernellCenter = data.ToList().IndexOf(data.Max());
        else kernellCenter = data.ToList().IndexOf(data.Min());
        //Para evitar indices negativos de data
        if (kernellCenter < kernellDistribution)
            Array.Copy(data, 0, kernell, 0, kernell.Length);
        //Para evitar indices mayores de data
        else if ((kernellCenter + kernell.Length - kernellDistribution) > data.Length)
            Array.Copy(data, (int)(data.Length - kernell.Length), kernell, 0, kernell.Length);
        else Array.Copy(data, (int)(kernellCenter - kernellDistribution), kernell, 0, kernell.Length);
        //Vuelvo a sumar la señal DC
        for (int i = 0; i < kernell.Length; i++)
            kernell[i] = kernell[i] + aver;


        GetComponent<AccCapturer>().SaveTemplate(string.Join(" ", kernell.Select(x => x.ToString()).ToArray()));
        //state.text = ("Kernell created from: " + kernellCenter.ToString());
    }

    void MarchCaptureAcc()
    {
        if (greenLight)
        {
            xAcc = Input.acceleration.x;
            yAcc = Input.acceleration.y;
            zAcc = Input.acceleration.z;
            if (kernellTime < (oneStep * initStepCount))    //Espera 6 segundos a que capture datos de los acelerometros en cada array
            {
                //state.text = ("Adquiring data for Kernell");
                xKernellData = xKernellData.Concat(new float[] { xAcc }).ToArray();
                yKernellData = yKernellData.Concat(new float[] { yAcc }).ToArray();
                zKernellData = zKernellData.Concat(new float[] { zAcc }).ToArray();
            }
            else
            {
                if (capKernell)     //Esto solo ocurre una vez para seleccionar el kernell de los datos almacenados
                {
                    capKernell = false;
                    //Detectar kernell
                    //Primero sobre el eje con mayor dispercion
                    float Average = xKernellData.Average();
                    float sumOfSquaresOfDifferences = xKernellData.Select(val => (val - Average) * (val - Average)).Sum();
                    double xSd = Math.Sqrt(sumOfSquaresOfDifferences / xKernellData.Length);
                    Average = yKernellData.Average();
                    sumOfSquaresOfDifferences = yKernellData.Select(val => (val - Average) * (val - Average)).Sum();
                    double ySd = Math.Sqrt(sumOfSquaresOfDifferences / yKernellData.Length);
                    Average = zKernellData.Average();
                    sumOfSquaresOfDifferences = zKernellData.Select(val => (val - Average) * (val - Average)).Sum();
                    double zSd = Math.Sqrt(sumOfSquaresOfDifferences / zKernellData.Length);
                    //state.text = ("Std" + xSd.ToString() + ySd.ToString() + zSd.ToString());
                    if (ySd > xSd && ySd > zSd)
                    {
                        CaptureKernell(yKernellData);
                        //state.text = ("Y selected");
                        accSelected = new Vector3(0, 1, 0);
                    }
                    else if (zSd > xSd && zSd > ySd)
                    {
                        CaptureKernell(zKernellData);
                        //state.text = ("Z selected");
                        accSelected = new Vector3(0, 0, 1);
                    }
                    else
                    {
                        CaptureKernell(xKernellData);
                        //state.text = ("X selected");
                        accSelected = new Vector3(1, 0, 0);
                    }
                    //state.text = ("Kernell: " + string.Join(",", kernell.Select(p => p.ToString()).ToArray()));
                }
                else    // After having the kernell, it is used to counter steps
                {
                    //Detectar pasos con el kernell
                    //state.text = ("Using Kernell");
                    Array.Copy(realTimeData, 0, realTimeData, 1, realTimeData.Length - 1);
                    realTimeData[0] = xAcc * accSelected.x + yAcc * accSelected.y + zAcc * accSelected.z;
                    //state.text = ("Real Time Data: " + string.Join(",", realTimeData.Select(p => p.ToString()).ToArray()));
                    float result;
                    unsafe
                    {
                        fixed (float* src = &realTimeData[0])
                        {
                            fixed (float* tmp = &kernell[0])
                            {
                                result = OcvMechanics.MatchTemplate(src, realTimeData.Length, tmp, kernell.Length);
                                stepRestriction++;      //No cuenta pasos dentro de 0.5s despupes del anterior
                            }
                        }
                    }
                    if (result > presision && stepRestriction > 5)
                    {
                        steps++;
                        stepRestriction = 0;
                    }
                    if (stepRestriction > 40)   // Reinicia la captura del kernell después de 3 segundos
                    {
                        ResetKernell();
                        //Debug.Log("Kernell lost, getting new");
                    }
                }
            }
        }
    }

    private void ResetKernell()
    {
        StartCapturing();
        initDelay = 0;
        StartCapturing();
    }
}
