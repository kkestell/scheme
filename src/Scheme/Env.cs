namespace Scheme;

public class Env
{
    private readonly Dictionary<string, Obj> _env = new();
    private readonly Env? _outer;

    public Env()
    {
        RegisterBuiltins();
        RegisterSpecialForms();
    }

    public Env(Env outer)
    {
        _outer = outer;
    }

    public void Define(string name, Obj value)
    {
        _env[name] = value;
    }

    public void Set(string name, Obj value)
    {
        _env[name] = value;
    }

    public Obj? Lookup(string name)
    {
        return _env.TryGetValue(name, out var value) ? value : _outer?.Lookup(name);
    }

    public Obj Eval(Obj expr)
    {
        switch (expr)
        {
            case NumberObj numberObj:
                return numberObj;
            case SymbolObj symbolObj:
            {
                var value = Lookup(symbolObj.Value);
                if (value == null) 
                    throw new Exception($"Undefined symbol: {symbolObj.Value}");
                return value;
            }
            case StringObj stringObj:
                return stringObj;
            case BooleanObj booleanObj:
                return booleanObj;
            case ClosureObj closureObj:
                return closureObj;
            case ListObj listObj:
                return EvalList(listObj);
            default:
                throw new Exception($"Unsupported expression type: {expr.GetType().Name}");
        }
    }

    private Obj EvalList(ListObj listObj)
    {
        if (listObj.Value.Count == 0)
            return listObj;

        var first = listObj.First;
        var evaluatedFirst = Eval(first);

        switch (evaluatedFirst)
        {
            case SpecialFormObj specialForm:
            {
                var args = listObj.Rest;
                return specialForm.SpecialForm(new ListObj(args), this);
            }
            case FunctionObj function:
            {
                var args = listObj.Rest.Select(Eval).ToList();
                return function.Func(new ListObj(args));
            }
            case ClosureObj closure:
            {
                var newEnv = new Env(closure.Env);
                var parameters = closure.Params.Value;
                var args = listObj.Rest.Select(Eval).ToList();

                if (parameters.Count != args.Count)
                    throw new Exception("Argument count mismatch");

                for (var i = 0; i < parameters.Count; i++)
                {
                    if (parameters[i] is SymbolObj param)
                        newEnv.Define(param.Value, args[i]);
                    else
                        throw new Exception("Invalid parameter type");
                }

                return newEnv.Eval(closure.Body);
            }
            default:
                throw new Exception($"Cannot apply {first.GetType().Name} as a function.");
        }
    }
    
    private void RegisterBuiltins()
    {
        var builtins = new Dictionary<string, Func<Obj, Obj>>
        {
            // Arithmetic
            { "+", BuiltinsX.Add },
            { "-", BuiltinsX.Sub },
            { "*", BuiltinsX.Mul },
            { "/", BuiltinsX.Div },
            // Comparison
            { "=", BuiltinsX.Equal },
            { "<", BuiltinsX.LessThan },
            { ">", BuiltinsX.GreaterThan },
            { "<=", BuiltinsX.LessThanOrEqual },
            { ">=", BuiltinsX.GreaterThanOrEqual },
            // Boolean
            { "not", BuiltinsX.Not },
            // List
            { "car", BuiltinsX.Car },
            { "cdr", BuiltinsX.Cdr },
            { "cons", BuiltinsX.Cons },
            { "list", BuiltinsX.CreateList },
            { "map", BuiltinsX.Map },
            { "filter", BuiltinsX.Filter },
            { "reduce", BuiltinsX.Reduce },
            // Type
            { "null?", BuiltinsX.IsNull },
            // IO
            { "display", BuiltinsX.Display },
            { "newline", BuiltinsX.Newline },
        };
        
        foreach (var (name, builtin) in builtins)
        {
            Define(name, new FunctionObj(builtin));
        }
    }

    private void RegisterSpecialForms()
    {
        var specialForms = new Dictionary<string, Func<Obj, Env, Obj>>
        {
            { "and", SpecialForms.And },
            { "begin", SpecialForms.Begin },
            { "cond", SpecialForms.Cond },
            { "define", SpecialForms.Define },
            { "if", SpecialForms.If },
            { "lambda", SpecialForms.Lambda },
            { "let", SpecialForms.Let },
            { "or", SpecialForms.Or },
            { "quote", SpecialForms.Quote },
            { "set!", SpecialForms.Set },
            { "while", SpecialForms.While },
        };
        
        foreach (var (name, specialForm) in specialForms)
        {
            Define(name, new SpecialFormObj(specialForm));
        }
    }
}