# Accelerate mongo db library
## Introduction
This library help us to increase the speed for develop repositories for data access with entity framework, providing classes for that.  
## How to install
For installing this package, you must execute the following command:  
```
dotnet add package accelerate.data.entityframework
```
## How to use
### Writing a custom data repository options
``` csharp
public class MyRepositoryOptions : EntityFrameworkRepositoryOptions
{
    // Declare properties to expose
}
```
### Writing a custom data repository context
``` csharp
public class MyRepositoryContext<TEntity, TOptions> : EntityFrameworkRepositoryContext<TEntity, TOptions> where TEntity : Entity, new() where TOptions : MyRepositoryOptions
{
    private Boolean _disposed;

    /// <summary>
    /// Initialize a new instance of class <seealso cref="MyRepositoryContext{TEntity, TOptions}" />.
    /// </summary>
    /// <param name="repository">
    /// Repository managed in the context.
    /// </param>
    /// <param name="options">
    /// Configuration options of repository.
    /// </param>
    protected MyRepositoryContext(MyRepository<TEntity, TOptions> repository, TOptions options) : base(repository, options)
    {
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">
    /// Indicate if object is currently freeing, releasing, or resetting unmanaged resources.
    /// </param>
    protected virtual void Dispose(Boolean disposing)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(this.GetType().Name);
        }

        base.Dispose(disposing);

        if (disposing)
        {
            // Free resources memory allocation
        }

        _disposed = true;
    }
}
```
### Writing a custom data repository
``` csharp
public abstract class MyRepository : EntityFrameworkRepository<MyEntity, MyRepositoryOptions>
{
    private Boolean _disposed;

    /// <summary>
    /// Initialize a new instance of <seealso cref="MyRepository" /> class.
    /// </summary>
    /// <param name="options">
    /// Configuration options of data repository.
    /// </param>
    public MyRepository(IOptions<MyRepositoryOptions> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void Dispose(Boolean disposing)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }

        base.Dispose(disposing);

        if (disposing)
        {
            // Free resources memory allocation
        }

        _disposed = true;
    }
}
```
## Changes history
**Version 6.0.0**
- Changed version to a system based on .NET Core version supported.  
**Version 1.0.0**
- Include base classes for entities, repositories and repository options.  