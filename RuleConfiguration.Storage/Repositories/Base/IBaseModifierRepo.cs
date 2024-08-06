namespace RuleConfiguration.Storage.Repositories.Base;

public interface IBaseModifierRepo<T> where T : class
{
    Task<T> ApplyModifiers(T data);

    List<IBaseModifier<T>> AllModifiers { get; set; }
}