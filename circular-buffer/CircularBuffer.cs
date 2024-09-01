using CircularBuffer.Common.Constants;
using System;

public class CircularBuffer<T>
{
    private readonly T[] _buffer;
    private int _head;
    private int _tail;
    private int _size;

    public CircularBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException(ErrorTokens.BufferCapacityError, nameof(capacity));

        _buffer = new T[capacity];
        _head = 0;
        _tail = 0;
        _size = 0;
    }

    public int Capacity => _buffer.Length;
    public int Size => _size;
    public bool IsEmpty => _size == 0;
    public bool IsFull => _size == Capacity;

    public T Read()
    {
        if (IsEmpty)
            throw new InvalidOperationException(ErrorTokens.BufferIsEmptyError);

        var value = _buffer[_head];
        _buffer[_head] = default(T); // Optional: Clear the slot for GC.
        _head = (_head + 1) % Capacity;
        _size--;

        return value;
    }

    public void Write(T value)
    {
        if (IsFull)
            throw new InvalidOperationException(ErrorTokens.BufferIsFullError);

        _buffer[_tail] = value;
        _tail = (_tail + 1) % Capacity;
        _size++;
    }

    public void Overwrite(T value)
    {
        if (IsFull)
        {
            // Advance head to overwrite the oldest element.
            _head = (_head + 1) % Capacity;
        }
        else
        {
            _size++;
        }

        _buffer[_tail] = value;

        // Advance tail to the next position.
        _tail = (_tail + 1) % Capacity;
    }

    public void Clear()
    {
        _head = 0;
        _tail = 0;
        _size = 0;

        // Optional: Clear all elements.
        Array.Clear(_buffer, 0, Capacity);
    }
}