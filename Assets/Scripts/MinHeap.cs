using System;
using System.Collections.Generic;

//Reference: https://www.geeksforgeeks.org/binary-heap/
public class MinHeap<T>
{
    private readonly int maxSize;
    private int currentSize;
    private readonly T[] arr;

    public delegate float ValueFunction(T obj);
    private readonly ValueFunction valueFunc;

    public MinHeap(int size, ValueFunction valueFunc)
    {
        maxSize = size;
        currentSize = 0;
        arr = new T[size];
        this.valueFunc = valueFunc;
    }

    // Get the current size of the heap
    public int Count()
    {
        return currentSize;
    }

    // Swap the position of elements in the heap using reference
    private void Swap(ref T x, ref T y)
    {
        T temp = x;
        x = y;
        y = temp;
    }

    // Get the Parent index for given index
    private int Parent(int index)
    {
        if (index == 0)
        {
            throw new Exception("Root node of the heap does not have Parent");
        }
        return (index - 1) / 2;
    }

    // Get the Left Child index for given index
    private int Left(int index)
    {
        return index * 2 + 1;
    }

    // Get the Right Child index for given index
    private int Right(int index)
    {
        return index * 2 + 2;
    }

    // Insert new element on the heap
    public void Add(T newElement)
    {
        if (currentSize == maxSize)
        {
            throw new Exception("Heap is full and cannot add new elements");
        }

        // Insert new element at the end
        int i = currentSize;
        arr[i] = newElement;
        currentSize++;

        // Fix heap property if violated
        while (i != 0 && valueFunc(arr[i]) < valueFunc(arr[Parent(i)]))
        {
            Swap(ref arr[i], ref arr[Parent(i)]);
            i = Parent(i);
        }
    }

    // Insert all elements from a list on the heap and build the heap
    //public void Add(List<T> list)
    //{
    //    if (list.Count > maxSize)
    //    {
    //        throw new Exception("List with size greater than the heap");
    //    }

    //    foreach (var e in list)
    //    {
    //        Add(e);
    //    }

    //    BuildHeap();
    //}

    // Returns the minium value element from the heap
    public T GetMin()
    {
        return arr[0];
    }

    // Remove the minium value element from the heap
    public T RemoveMin()
    {
        if (currentSize == 0)
        {
            throw new Exception("Heap is empty and cannot remove any element");
        }

        if (currentSize == 1)
        {
            currentSize--;
            return arr[0];
        }

        T root = arr[0];

        arr[0] = arr[currentSize - 1];
        currentSize--;
        Heapify(0);

        return root;
    }

    // Recursive method to guarantee heap property on subtree with given root index.
    // Assumes that the subtrees are already heapfied
    private void Heapify(int rootIndex)
    {
        int l = Left(rootIndex);
        int r = Right(rootIndex);

        int smallest = rootIndex;
        if (l < currentSize && valueFunc(arr[l]) < valueFunc(arr[smallest]))
        {
            smallest = l;
        }
        if (r < currentSize && valueFunc(arr[r]) < valueFunc(arr[smallest]))
        {
            smallest = r;
        }

        if (smallest != rootIndex)
        {
            Swap(ref arr[rootIndex], ref arr[smallest]);
            Heapify(smallest);
        }
    }

    // Build the heap from the array
    public void BuildHeap()
    {
        // Index of the last non leaf element
        int startIndex = currentSize > 1 ? 0 : (currentSize / 2) - 1;

        // Reverse heapify on each non leaf node
        for (int i = startIndex; i >= 0; i--)
        {
            Heapify(i);
        }
    }
}
