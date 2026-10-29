using System;
using System.Collections;
using UnityEngine;

public class PiggyBank : MonoBehaviour
{
    [SerializeField] int maxCapacity=10;

    [SerializeField] int currentValue;

    [SerializeField] BankAccount bankAccount;

    private void Awake()
    {
        currentValue = 0;

    }
    private void Start()
    {
        StartCoroutine(AddMoneyToPiggyBank());
    }

    IEnumerator AddMoneyToPiggyBank()
    {
        yield return new WaitForSeconds(1);
        currentValue++;
        if (currentValue == maxCapacity)
        {
            AddToAccount();
        }
        StartCoroutine(AddMoneyToPiggyBank());// quando finisce chiama se stesso

    }
    void AddToAccount()
    {
        if (bankAccount)
        {
            bankAccount.AddToBank(currentValue);
            Debug.Log($"MONEY IN THE BANK: {bankAccount.BankValue}");
            currentValue = 0; //serve per resettare

        }
    }
}
