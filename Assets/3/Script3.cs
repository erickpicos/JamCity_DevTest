using LiteNetLib.Utils;
using UnityEngine;

public class Script3 : MonoBehaviour
{
    [SerializeField] private Transform transformDataToSend;
    [SerializeField] private Transform playerTransform;

    NetDataWriter dataWriter = new NetDataWriter();
    NetDataReader dataReader = new NetDataReader();

    private void Update()
    {
        // *** Don't modify this method. ***

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
        dataWriter.Put(transform.position.x);
        dataWriter.Put(transform.position.y);
        dataWriter.Put(transform.position.z);

        dataWriter.Put(transform.eulerAngles.x);
        dataWriter.Put(transform.eulerAngles.y);
        dataWriter.Put(transform.eulerAngles.z);
    }

    private void DeserializeTransform(Transform transform, NetDataReader dataReader)
    {
        Vector3 newPosition = new Vector3();
        newPosition.x = dataReader.GetFloat();
        newPosition.y = dataReader.GetFloat();
        newPosition.z = dataReader.GetFloat();

        Vector3 newRotation = new Vector3();
        newRotation.x = dataReader.GetFloat();
        newRotation.y = dataReader.GetFloat();
        newRotation.z = dataReader.GetFloat();

        transform.position = newPosition;
        transform.eulerAngles = newRotation;
    }
}