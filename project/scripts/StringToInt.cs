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
        StringBuilder result = new StringBuilder();
        Debug.Log('"'+input+'"'+ input == ""+' '+input.Length+' '+string.IsNullOrEmpty(input));
        if (input == "")
        {
            Debug.Log(1223334444);
            input = " ";
        }
        Debug.Log(12312313123123123131);
        for (int i = 0; i < input.Length - 1; i++)
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

        // ����������� ������ � BigInteger
        return resultat;
    }
}