using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T: IHeapItem<T>
{

    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }


    public T RemoveFirst()
    {
        T first = items[0];
        currentItemCount--;
        T last = items[currentItemCount];
        last.HeapIndex = first.HeapIndex;
        items[0] = last;

        SortDown(last);
        return first;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
        
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }


    public void SortDown(T item)
    {
        while (true)
        {
            int leftChild = 2 * item.HeapIndex + 1;
            int rightChild = 2 * item.HeapIndex + 2;
            int swapIndex;

            if(leftChild < currentItemCount)
            {
                swapIndex = leftChild;

                if(rightChild < currentItemCount)
                {
                    if (items[leftChild].CompareTo(items[rightChild]) < 0)
                    {
                        swapIndex = rightChild;
                    }
                }

                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }


        }
    }

    public void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem)> 0)
            {
                // swap, item has higher priority which means lower f cost
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    public void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAtmp = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAtmp;
    }

}


public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}