using RuleConfiguration.Engine.Interfaces;

namespace RuleConfiguration.Engine.Exceptions;

/// <summary>
///     Represents an attempt to use an operation providing the wrong number of values.
/// </summary>
[Serializable]
public class WrongNumberOfValuesException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WrongNumberOfValuesException" /> class.
    /// </summary>
    /// <param name="operation">Operation used.</param>
    public WrongNumberOfValuesException(IOperation operation)
    {
        Operation = operation;
    }

    /// <summary>
    ///     Gets the <see cref="Operation" /> attempted to be used.
    /// </summary>
    public IOperation Operation { get; private set; }

    /// <summary>
    ///     Gets a message that describes the current exception.
    /// </summary>
    public override string Message =>
        string.Format("The operation '{0}' admits exactly '{1}' values (not more neither less than this).",
            Operation.Name, Operation.NumberOfValues);
}