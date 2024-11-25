using System;
using System.Text;
using System.Numerics;
using UnityEngine;

public class StringToNumberConverter
{
    public static int ConvertBigIntegerToInt(BigInteger bigInteger)
    {
        if (bigInteger < int.MinValue || bigInteger > int.MaxValue)
        {
            return UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
            return (int)bigInteger; // ����������� BigInteger � int
    }
    public static int ConvertStringToNumbers(string input)
    {
        byte f = 0;
        StringBuilder result = new StringBuilder();
        if (input == "")
        {
            input = " ";
        }
        if (input[0] == '-' && input.Length>2)
        {
            f = 1;
        }
        for (int i = f; i < input.Length - 1; i++)
        {
            char c = input[i];
            if (char.IsDigit(c))
            {
                // ���� ������ - �����, ��������� ��� � ���������
                result.Append(c);
            }
            else
            {
                // ���� ������ - �� �����, ����������� ��� � ��� ������� ����� 32
                int unicodeValue = (int)c - 31;
                result.Append(unicodeValue);
            }
            //Debug.Log(result);
        }

        BigInteger res = BigInteger.Parse(result.ToString());
        //Debug.Log(res);
        int resultat = ConvertBigIntegerToInt(res);
        if (f == 1)
        {
            resultat = resultat * -1;
        }

        // ����������� ������ � BigInteger
        return resultat;
    }
}