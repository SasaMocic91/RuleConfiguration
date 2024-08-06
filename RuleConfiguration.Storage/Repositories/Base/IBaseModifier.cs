namespace RuleConfiguration.Storage.Repositories.Base;

public interface IBaseModifier<T>
{
    T Modify(T data);

    string GetModifierName();
}