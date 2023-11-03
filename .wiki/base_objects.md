# Base objects for data layer
## Introduction
This library help us to increase the speed for develop repositories for data access, providing classes for that.  
## How to install
For installing this package, you must execute the following command:  
```
dotnet add package accelerate.data
```
## How to use
### Writing a custom data entity
``` csharp
public abstract class MyEntity : Entity
{
    private Boolean _disposed;

    /// <summary>
    /// Initialize a new instance of <see cref="MyEntity" /> class.
    /// </summary>
    protected MyEntity() : base()
    {
    }
    /// <inherithdoc />
    [ExcludeFromCodeCoverage]
    [Obsolete("This constructor is obsolete for security reasons")]
    protected MyEntity(SerializationInfo info, StreamingContext context) : base(info, context)
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
public abstract class MyRepositoryOptions : QueryableRepositoryOptions
{
    // Declare properties to expose
}
```
### Writing a custom data repository
``` csharp
public abstract class MyRepository<TEntity, TOptions> : QueryableRepository<TEntity, TOptions> where TEntity : MyEntity, new() where TOptions : MyRepositoryOptions
{
    private Boolean _disposed;

    /// <summary>
    /// Initialize a new instance of <seealso cref="MyRepository{TEntity,TOptions}" /> class.
    /// </summary>
    /// <param name="options">
    /// Configuration options of data repository.
    /// </param>
    protected MyRepository(IOptions<TOptions> options) : base(options)
    {
    }

    /// <inheritdoc />
    public override void Commit()
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override void Delete(TEntity entity)
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override void Delete(IEnumerable<TEntity> entities)
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override void Delete(Expression<Func<TEntity, Boolean>> expression)
    {
        // Add your custom code here
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
    /// <inheritdoc />
    public override void Insert(TEntity entity)
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override void Insert(IEnumerable<TEntity> entities)
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override void Rollback()
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override IEnumerable<TEntity> Select()
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression)
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip)
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override IEnumerable<TEntity> Select(Expression<Func<TEntity, Boolean>> expression, Int32 skip, Int32 take)
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override void Update(TEntity entity)
    {
        // Add your custom code here
    }
    /// <inheritdoc />
    public override void Update(IEnumerable<TEntity> entities)
    {
        // Add your custom code here
    }
}
```
## Changes history
**Version 6.0.0**
- Changed version to a system based on .NET Core version supported.  
**Version 1.0.0**
- Include base classes for entities, repositories and repository options.  