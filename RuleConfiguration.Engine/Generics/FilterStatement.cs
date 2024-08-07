using RuleConfiguration.Engine.Common;
using RuleConfiguration.Engine.Exceptions;
using RuleConfiguration.Engine.Helpers;
using RuleConfiguration.Engine.Interfaces;

namespace RuleConfiguration.Engine.Generics;

/// <summary>
///     Defines how a property should be filtered.
/// </summary>
[Serializable]
public class FilterStatement<TPropertyType> : IFilterStatement
{
    /// <summary>
    ///     Instantiates a new <see cref="FilterStatement{TPropertyType}" />.
    /// </summary>
    /// <param name="propertyId"></param>
    /// <param name="operation"></param>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <param name="connector"></param>
    public FilterStatement(string propertyId, IOperation operation, TPropertyType value, TPropertyType value2,
        Connector connector)
    {
        PropertyId = propertyId;
        Connector = connector;
        Operation = operation;
        SetValues(value, value2);
        Validate();
    }

    /// <summary>
    ///     Instantiates a new <see cref="FilterStatement{TPropertyType}" />.
    /// </summary>
    public FilterStatement()
    {
    }

    /// <summary>
    ///     Establishes how this filter statement will connect to the next one.
    /// </summary>
    public Connector Connector { get; set; }

    /// <summary>
    ///     Property identifier conventionalized by for the Expression Builder.
    /// </summary>
    public string PropertyId { get; set; }

    /// <summary>
    ///     Express the interaction between the property and the constant value defined in this filter statement.
    /// </summary>
    public IOperation Operation { get; set; }

    /// <summary>
    ///     Constant value that will interact with the property defined in this filter statement.
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    ///     Constant value that will interact with the property defined in this filter statement when the operation demands a
    ///     second value to compare to.
    /// </summary>
    public object Value2 { get; set; }

    /// <summary>
    ///     Validates the FilterStatement regarding the number of provided values and supported operations.
    /// </summary>
    public void Validate()
    {
        var helper = new OperationHelper();
        ValidateNumberOfValues();
        ValidateSupportedOperations(helper);
    }

    private void SetValues(TPropertyType value, TPropertyType value2)
    {
        if (typeof(TPropertyType).IsArray)
        {
            if (!Operation.SupportsLists)
                throw new ArgumentException("It seems the chosen operation does not support arrays as parameters.");

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(typeof(TPropertyType).GetElementType());
            Value = value != null ? Activator.CreateInstance(constructedListType, value) : null;
            Value2 = value2 != null ? Activator.CreateInstance(constructedListType, value2) : null;
        }
        else
        {
            Value = value;
            Value2 = value2;
        }
    }

    private void ValidateNumberOfValues()
    {
        var numberOfValues = Operation.NumberOfValues;
        var failsForSingleValue = numberOfValues == 1 && !Equals(Value2, default(TPropertyType));
        var failsForNoValueAtAll = numberOfValues == 0 &&
                                   ( !Equals(Value, default(TPropertyType)) ||
                                     !Equals(Value2, default(TPropertyType)) );

        if (failsForSingleValue || failsForNoValueAtAll) throw new WrongNumberOfValuesException(Operation);
    }

    private void ValidateSupportedOperations(OperationHelper helper)
    {
        if (typeof(TPropertyType) == typeof(object))
        {
             // Ignore type object, operations are not supported.
            return;
        }

        var supportedOperations = helper.SupportedOperations(typeof(TPropertyType));

        if (!supportedOperations.Contains(Operation))
            throw new UnsupportedOperationException(Operation, typeof(TPropertyType).Name);
    }

    /// <summary>
    ///     String representation of <see cref="FilterStatement{TPropertyType}" />.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        switch (Operation.NumberOfValues)
        {
            case 0:
                return string.Format("{0} {1}", PropertyId, Operation);

            case 2:
                return string.Format("{0} {1} {2} And {3}", PropertyId, Operation, Value, Value2);

            default:
                return string.Format("{0} {1} {2}", PropertyId, Operation, Value);
        }
    }
}