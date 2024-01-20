namespace Scheme;

public abstract class Obj
{
    public override bool Equals(object? obj)
    {
        return obj is Obj other && Equals(other);
    }

    protected virtual bool Equals(Obj other)
    {
        return GetType() == other.GetType();
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }
}

public class SymbolObj : Obj
{
    public string Value { get; }

    public SymbolObj(string value) => Value = value;
    
    public override string ToString() => Value;
    
    protected override bool Equals(Obj other)
    {
        return other is SymbolObj symbolObj && Value == symbolObj.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class BooleanObj : Obj
{
    public bool Value { get; }

    public BooleanObj(bool value) => Value = value;
    
    public override string ToString() => Value ? "#t" : "#f";
    
    protected override bool Equals(Obj other)
    {
        return other is BooleanObj booleanObj && Value == booleanObj.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class NumberObj : Obj
{
    public double Value { get; }

    public NumberObj(double value) => Value = value;
    
    public override string ToString() => Value.ToString();
    
    protected override bool Equals(Obj other)
    {
        return other is NumberObj numberObj && Value == numberObj.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class StringObj : Obj
{
    public string Value { get; }

    public StringObj(string value) => Value = value;
    
    public override string ToString() => Value;
    
    protected override bool Equals(Obj other)
    {
        return other is StringObj stringObj && Value == stringObj.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class ListObj : Obj
{
    public List<Obj> Value { get; }

    public List<Obj> Rest => Value.Skip(1).ToList();
    
    public Obj First => Value[0];

    public ListObj(List<Obj> value)
    {
        Value = value;
    }

    public override string ToString() => $"({string.Join(" ", Value)})";
    
    protected override bool Equals(Obj other)
    {
        if (other is not ListObj listObj) return false;
        return Value.SequenceEqual(listObj.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class FunctionObj : Obj
{
    public Func<Obj, Obj> Func { get; }

    public FunctionObj(Func<Obj, Obj> func) => Func = func;
    
    public override string ToString() => "<function>";
    
    protected override bool Equals(Obj other)
    {
        return other is FunctionObj;
    }

    public override int GetHashCode()
    {
        return Func.GetHashCode();
    }
}

public class SpecialFormObj : Obj
{
    public Func<Obj, Env, Obj> SpecialForm { get; }

    public SpecialFormObj(Func<Obj, Env, Obj> specialForm) => SpecialForm = specialForm;
    
    public override string ToString() => "<special-form>";
    
    protected override bool Equals(Obj other)
    {
        return other is SpecialFormObj;
    }

    public override int GetHashCode()
    {
        return SpecialForm.GetHashCode();
    }
}

public class ClosureObj : Obj
{
    public ListObj Params { get; }
    public Obj Body { get; }
    public Env Env { get; }

    public ClosureObj(ListObj parameters, Obj body, Env env)
    {
        Params = parameters;
        Body = body;
        Env = env;
    }
    
    public override string ToString() => "<closure>";
    
    protected override bool Equals(Obj other)
    {
        return other is ClosureObj closureObj && Params.Equals(closureObj.Params) && Body.Equals(closureObj.Body);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Params, Body, Env);
    }    
}