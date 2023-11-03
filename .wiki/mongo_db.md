# Accelerate mongo db library
## Introduction
This library help us to increase the speed for develop repositories for data access with mongo db, providing classes for that.  
## How to install
For installing this package, you must execute the following command:  
```
dotnet add package accelerate.data.mongo
```
## How to use
### Writing a custom data entity
``` csharp
public class MyEntity : MongoEntity
{
    private Boolean _disposed;

    /// <summary>
    /// Initialize a new instance of <see cref="MyEntity" /> class.
    /// </summary>
    public MyEntity() : base()
    {
    }
    /// <inherithdoc />
    [ExcludeFromCodeCoverage]
    [Obsolete("This constructor is obsolete for security reasons")]
    public MyEntity(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    // Declare properties to expose

    /// <inherithdoc />
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
### Writing a custom data repository options
``` csharp
public class MyRepositoryOptions : MongoRepositoryOptions
{
    // Declare properties to expose
}
```
### Writing a custom data repository
``` csharp
public class MyRepository : MongoRepository<MyEntity, MyRepositoryOptions>
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