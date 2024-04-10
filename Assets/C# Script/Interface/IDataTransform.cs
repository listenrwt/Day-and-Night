using UnityEngine;

public interface IDataTransferable<T>
{
    T Send();
    void Recieve(T data);
}

