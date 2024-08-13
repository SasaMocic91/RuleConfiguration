namespace RuleConfiguration.Engine.Interfaces;

/// <summary>
///     Defines a filter from which a expression will be built.
/// </summary>
public interface IFilter
{
    IFilterStatement Statement { get; set; }

    /// <summary>
    ///     Add a statement, that doesn't need value, to this filter.
    /// </summary>
    /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
    /// <param name="operation">Express the interaction between the property and the constant value.</param>
    /// <returns>A FilterStatementConnection object that defines how this statement will be connected to the next one.</returns>
    IFilterStatement By(string propertyId, IOperation operation);

    /// <summary>
    ///     Adds another statement to this filter.
    /// </summary>
    /// <param name="propertyId">Name of the property that will be filtered.</param>
    /// <param name="operation">Express the interaction between the property and the constant value.</param>
    /// <param name="value">
    ///     Constant value that will interact with the property, required by operations that demands one value
    ///     or more.
    /// </param>
    /// <param name="value2">
    ///     Constant value that will interact with the property, required by operations that demands two
    ///     values.
    /// </param>
    /// <returns>A FilterStatementConnection object that defines how this statement will be connected to the next one.</returns>
    IFilterStatement By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value);
    
    /// <summary>
    ///     Adds another statement to this filter.
    /// </summary>
    /// <param name="propertyId">Name of the property that will be filtered.</param>
    /// <param name="operation">Express the interaction between the property and the constant value.</param>
    /// <param name="value">
    ///     Constant value that will interact with the property, required by operations that demands one value
    ///     or more.
    /// </param>
    /// <param name="value2">
    ///     Constant value that will interact with the property, required by operations that demands two
    ///     values.
    /// </param>
    /// <returns>A FilterStatementConnection object that defines how this statement will be connected to the next one.</returns>
    IFilterStatement By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value,
        TPropertyType value2);
}