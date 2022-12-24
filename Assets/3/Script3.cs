using System;
using System.Linq;
using LiteNetLib.Utils;
using UnityEngine;

public class Script3 : MonoBehaviour
{
    private Vector3 lastTransformPosition = new Vector3();
    private Vector3 lastTransformRotation = new Vector3();
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
        
        // Now, we deserialize the data back, and use it to set the playerTransform. They should match.
        dataReader.SetSource(data);
        DeserializeTransform(playerTransform, dataReader);
    }

    private void SerializeTransform(Transform transform, NetDataWriter dataWriter)
    {
        string binario = InfoDataToSend();
        byte decimalToSend = (byte) BinarioToDecimal(binario);
        
        dataWriter.Put(decimalToSend);
        if(binario[5] == '1') dataWriter.Put((sbyte)(transform.position.x * 25));
        if(binario[4] == '1') dataWriter.Put((sbyte)(transform.position.y * 25));
        if(binario[3] == '1') dataWriter.Put((sbyte)(transform.position.z * 25));
        if(binario[2] == '1') dataWriter.Put((short)(transform.eulerAngles.x * 80));
        if(binario[1] == '1') dataWriter.Put((short)(transform.eulerAngles.y * 80));
        if(binario[0] == '1') dataWriter.Put((short)(transform.eulerAngles.z * 80));

        lastTransformPosition = new Vector3((sbyte)(transform.position.x * 25), (sbyte)(transform.position.y * 25), (sbyte)(transform.position.z * 25));
        lastTransformRotation = new Vector3(
            (short)(transform.eulerAngles.x * 80),
            (short)(transform.eulerAngles.y * 80), 
            (short)(transform.eulerAngles.z * 80));
    }

    private void DeserializeTransform(Transform transform, NetDataReader dataReader)
    {
        byte decimalReceived = dataReader.GetByte();
        string binario = DecimalToBinario(decimalReceived);

        Vector3 newPosition = new Vector3();
        if (binario[5] == '1') {newPosition.x = dataReader.GetSByte() / 25f;} else {newPosition.x = transform.localPosition.x;}
        if (binario[4] == '1') {newPosition.y = dataReader.GetSByte() / 25f;} else {newPosition.y = transform.localPosition.y;}
        if (binario[3] == '1') {newPosition.z = dataReader.GetSByte() / 25f;} else {newPosition.z = transform.localPosition.z;}
        Vector3 newRotation = new Vector3();
        if (binario[2] == '1') {newRotation.x = dataReader.GetShort() / 80f;} else {newRotation.x = transform.eulerAngles.x;}
        if (binario[1] == '1') {newRotation.y = dataReader.GetShort() / 80f;} else {newRotation.y = transform.eulerAngles.y;}
        if (binario[0] == '1') {newRotation.z = dataReader.GetShort() / 80f;} else {newRotation.z = transform.eulerAngles.z;}
        
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
        if ((sbyte)(transformDataToSend.position.x * 25) != lastTransformPosition.x) info += 1;
        if ((sbyte)(transformDataToSend.position.y * 25) != lastTransformPosition.y) info += 10;
        if ((sbyte)(transformDataToSend.position.z * 25) != lastTransformPosition.z) info += 100;
        if ((short)(transformDataToSend.eulerAngles.x * 80) != lastTransformRotation.x) info += 1000;
        if ((short)(transformDataToSend.eulerAngles.y * 80) != lastTransformRotation.y) info += 10000;
        if ((short)(transformDataToSend.eulerAngles.z * 80) != lastTransformRotation.z) info += 100000;
        return info.ToString("D6");
    }
    
    
}