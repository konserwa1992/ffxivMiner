#include "pch.h"
#include "GameOutMethods.h"
#include <exception>
#include <typeinfo>
#include <string>




long long GetBaseAdress()
{
    // return BaseAddres;
    return (uintptr_t)GetModuleHandle(L"ffxiv.exe");
}



long long GetInt64(uintptr_t adress)
{
    return *reinterpret_cast<long long*>(adress);
}


int GetInt32(uintptr_t adress)
{
    return *reinterpret_cast<int*>(adress);
}

float GetFloat(uintptr_t adress)
{
        return *reinterpret_cast<float*>(adress);
}


short GetShort(uintptr_t adress)
{
    return *reinterpret_cast<short*>(adress);
}


BYTE GetByte(uintptr_t adress)
{
    return *reinterpret_cast<BYTE*>(adress);
}

void GetByteArray(uintptr_t adress, char* outTable, int size)
{
    memcpy(outTable, reinterpret_cast<char*>(adress), size);
}

