namespace Disposable;

public sealed class DisposableService : IDisposable
{
    // Dispose method is not called when exception in ctor is thrown
    public DisposableService()
    {
        // throw new Exception();
    }
    
    // Dispose method is called when exception in ctor is thrown
    public void Method()
    {
        throw new Exception();
    }
    
    public void Dispose()
    {
        Console.WriteLine("I am disposing...");
    }
}