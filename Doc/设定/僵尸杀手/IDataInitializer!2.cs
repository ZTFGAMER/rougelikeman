using System;

public interface IDataInitializer<T, U>
{
    U GetInitializedType();
    bool HasType(int index);
    T Initialize(int index);
}

