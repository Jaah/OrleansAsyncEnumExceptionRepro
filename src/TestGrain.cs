namespace OrleansAsyncEnumExceptionRepro;

public interface ITest : IGrainWithStringKey
{
    IAsyncEnumerable<string> List();
}

public class TestGrain : IGrainBase, ITest
{
    public IGrainContext GrainContext { get; }

    public TestGrain(IGrainContext context)
    {
        GrainContext = context;
    }
   
    public async IAsyncEnumerable<string> List()
    {
        // Pretend to do some work ...
        await Task.Delay(1000);
        var data = new[]
        {
            "start",
            "1",
            "2",
            "3",
            "end",
        };

        foreach (var s in data)
        {
            if (s == "end")
            {
                // Caller never gets this exception
                throw new MyException("Something went wrong in grain");
            }

            yield return s;
        }
    }
}

[GenerateSerializer]
public class MyException : Exception
{
    public MyException()
    {
    }

    public MyException(string message) : base(message)
    {
    }

    public MyException(string message, Exception inner) : base(message, inner)
    {
    }
}