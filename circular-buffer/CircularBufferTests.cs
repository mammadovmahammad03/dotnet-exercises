using CircularBuffer.Common.Constants;
using System;
using Xunit;

public class CircularBufferTests
{
    [Fact]
    public void ReadingEmptyBufferShouldFail()
    {
        var buffer = new CircularBuffer<int>(capacity: 1);
        var exception = Assert.Throws<InvalidOperationException>(() => buffer.Read());

        Assert.Equal(ErrorTokens.BufferIsEmptyError, exception.Message);
    }

    public void CanReadItemJustWritten()
    {
        var buffer = new CircularBuffer<int>(capacity: 1);
        buffer.Write(1);
        var result = buffer.Read();
        Assert.Equal(1, result);
    }

    [Fact]
    public void ClearResetsBuffer()
    {
        var buffer = new CircularBuffer<int>(3);
        buffer.Write(1);
        buffer.Write(2);

        buffer.Clear();

        Assert.True(buffer.IsEmpty);
        Assert.Equal(0, buffer.Size);

        // Verify that reading throws an exception on an empty buffer
        var exception = Assert.Throws<InvalidOperationException>(() => buffer.Read());
        Assert.Equal(ErrorTokens.BufferIsEmptyError, exception.Message);
    }

    [Fact]
    public void SizeIncreasesDecreasesCorrectly()
    {
        var buffer = new CircularBuffer<int>(5);
        buffer.Write(1);
        buffer.Write(2);
        Assert.Equal(2, buffer.Size);
        buffer.Read();
        Assert.Equal(1, buffer.Size);
    }
}