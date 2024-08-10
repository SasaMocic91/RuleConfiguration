namespace RuleConfiguration.Engine.Interfaces;

/// <summary>
///     Defines how a property should be filtered.
/// </summary>
public interface IFilterStatement
{
    /// <summary>
    ///     Property identifier conventionalized by for the Expression Builder.
    /// </summary>
    string PropertyId { get; set; }

    /// <summary>
    ///     Express the interaction between the property and the constant value defined in this filter statement.
    /// </summary>
    IOperation Operation { get; set; }

    /// <summary>
    ///     Constant value that will interact with the property defined in this filter statement.
    /// </summary>
    object Value { get; set; }

    /// <summary>
    ///     Constant value that will interact with the property defined in this filter statement when the operation demands a
    ///     second value to compare to.
    /// </summary>
    object Value2 { get; set; }

    /// <summary>
    ///     Validates the FilterStatement regarding the number of provided values and supported operations.
    /// </summary>
    void Validate();
}