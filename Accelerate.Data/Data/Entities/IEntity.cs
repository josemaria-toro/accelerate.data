using System;
using System.Runtime.Serialization;

namespace Accelerate.Data.Entities
{
    /// <summary>
    /// Data entity.
    /// </summary>
    public interface IEntity : IDisposable, ISerializable, ICloneable
    {
    }
}
