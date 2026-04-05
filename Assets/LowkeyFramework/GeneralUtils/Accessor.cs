using System;
using System.Linq.Expressions;

public class Accessor<T>
{
    public Accessor(Expression<Func<T>> expression)
    {
        if(expression.Body is not MemberExpression memberExpression)
            throw new ArgumentException("expression must return a field or property");
        ParameterExpression parameterExpression = Expression.Parameter(typeof(T));

        setter = Expression.Lambda<Action<T>>(Expression.Assign(memberExpression, parameterExpression), parameterExpression).Compile();
        getter = expression.Compile();
    }

    public void Set(T value) => setter(value);
    public T Get() => getter();

    private readonly Action<T> setter;
    private readonly Func<T> getter;
}