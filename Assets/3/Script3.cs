using System;
using System.Linq;
using LiteNetLib.Utils;
using UnityEngine;

public class Script3 : MonoBehaviour
{
    [SerializeField] private Transform lastTransformDataSent;
    [SerializeField] private Transform transformDataToSend;
    [SerializeField] private Transform playerTransform;

    NetDataWriter dataWriter = new NetDataWriter();
    NetDataReader dataReader = new NetDataReader();

    
    private void Update()
    {   // *** Don't modify this method. ***
        // First, we serialize transformDataToSend in the data writer.
        dataWriter.Reset();
        SerializeTransform(transformDataToSend, dataWriter);

        byte[] data = dataWriter.CopyData();
        Debug.Log($"Serialized data. Total bytes: {data.Length}");
        
        //Una vez enviada la información se guarda para compararla con el siguiete paquete
        lastTransformDataSent.localPosition = transformDataToSend.localPosition;
        lastTransformDataSent.eulerAngles = transformDataToSend.eulerAngles;

        // Now, we deserialize the data back, and use it to set the playerTransform. They should match.
        dataReader.SetSource(data);
        DeserializeTransform(playerTransform, dataReader);
    }

    private void SerializeTransform(Transform transform, NetDataWriter dataWriter)
    {
        string binario = InfoDataToSend();
        byte decimalToSend = (byte) BinarioToDecimal(binario);
        
        //Debug.Log("Sent_Binario: "+binario+" Byte: "+decimalToSend);
        
        dataWriter.Put(decimalToSend);
        if(binario[5] == '1') dataWriter.Put(transform.position.x);
        if(binario[4] == '1') dataWriter.Put(transform.position.y);
        if(binario[3] == '1') dataWriter.Put(transform.position.z);

        if(binario[2] == '1') dataWriter.Put(transform.eulerAngles.x);
        if(binario[1] == '1') dataWriter.Put(transform.eulerAngles.y);
        if(binario[0] == '1') dataWriter.Put(transform.eulerAngles.z);
    }

    private void DeserializeTransform(Transform transform, NetDataReader dataReader)
    {
        byte decimalReceived = dataReader.GetByte();
        string binario = DecimalToBinario(decimalReceived);

        //Debug.Log("Rece_Binario: "+binario+" Byte: "+decimalReceived);
        
        Vector3 newPosition = new Vector3();
        if (binario[5] == '1') {newPosition.x = dataReader.GetFloat();} else {newPosition.x = transform.localPosition.x;}
        if (binario[4] == '1') {newPosition.y = dataReader.GetFloat();} else {newPosition.y = transform.localPosition.y;}
        if (binario[3] == '1') {newPosition.z = dataReader.GetFloat();} else {newPosition.z = transform.localPosition.z;}
        Vector3 newRotation = new Vector3();
        if (binario[2] == '1') {newRotation.x = dataReader.GetFloat();} else {newRotation.x = transform.eulerAngles.x;}
        if (binario[1] == '1') {newRotation.y = dataReader.GetFloat();} else {newRotation.y = transform.eulerAngles.y;}
        if (binario[0] == '1') {newRotation.z = dataReader.GetFloat();} else {newRotation.z = transform.eulerAngles.z;}
        
        transform.position = newPosition;
        transform.eulerAngles = newRotation;
    }

    private string DecimalToBinario(int numero)
    {
        if (numero == 0) { return "000000"; }
        
        string cadena = "";
        while (numero > 0)
        { 
            if (numero % 2 == 0) 
                cadena = "0" + cadena; 
            else 
                cadena = "1" + cadena;
                
            numero = (int)(numero / 2);
        }
        return cadena.PadLeft(6, '0');;
    }
    public int BinarioToDecimal(string numeroBinario)
    {
        int exponente = 0;
        int numero;
        int suma = 0;
        string numeroInvertido = new string(numeroBinario.Reverse().ToArray());
            
        for (int i = 0; i < numeroInvertido.Length; i++)
        {
            numero = Convert.ToInt32(numeroInvertido.Substring(i, 1));
            suma += numero * (int)Math.Pow(2, exponente);
            exponente++;
        }
        return suma;
    }

    public string InfoDataToSend()
    {
        int info = 0;
        if (transformDataToSend.position.x != lastTransformDataSent.position.x) info += 1;
        if (transformDataToSend.position.y != lastTransformDataSent.position.y) info += 10;
        if (transformDataToSend.position.z != lastTransformDataSent.position.z) info += 100;
        if (transformDataToSend.eulerAngles.x != lastTransformDataSent.eulerAngles.x) info += 1000;
        if (transformDataToSend.eulerAngles.y != lastTransformDataSent.eulerAngles.y) info += 10000;
        if (transformDataToSend.eulerAngles.z != lastTransformDataSent.eulerAngles.z) info += 100000;
        return info.ToString("D6");
    }
    
    
}