namespace Scheme;

public static class SpecialForms
{
    public static Obj And(Obj args, Env env)
    {
        Obj last = new SymbolObj("unspecified");
        if (args is not ListObj listArgs)
            return last;

        foreach (var arg in listArgs.Value)
        {
            last = env.Eval(arg);
            if (last is BooleanObj { Value: false })
                return last;
        }
        return last;
    }

    public static Obj Begin(Obj args, Env env)
    {
        Obj last = new SymbolObj("unspecified");
        if (args is not ListObj listArgs)
            return last;

        foreach (var expr in listArgs.Value)
        {
            last = env.Eval(expr);
        }
        return last;
    }

    public static Obj Cond(Obj args, Env env)
    {
        if (args is not ListObj listArgs)
            return new SymbolObj("unspecified");

        foreach (var clause in listArgs.Value)
        {
            if (clause is not ListObj clauseList || clauseList.Value.Count != 2)
                throw new ArgumentException("Each clause must be a list with exactly 2 elements.");

            var condition = env.Eval(clauseList.Value[0]);
            if (condition is BooleanObj boolCondition && boolCondition.Value)
                return env.Eval(clauseList.Value[1]);
        }
        return new SymbolObj("unspecified");
    }

    public static Obj Define(Obj args, Env env)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2 || listArgs.Value[0] is not SymbolObj)
            throw new ArgumentException("define requires exactly 2 arguments, and the first must be a symbol.");

        var name = ((SymbolObj)listArgs.Value[0]).Value;
        var value = env.Eval(listArgs.Value[1]);
        env.Define(name, value);

        return new SymbolObj("unspecified");
    }

    public static Obj If(Obj args, Env env)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count < 2 || listArgs.Value.Count > 3)
            throw new ArgumentException("if requires 2 or 3 arguments.");

        var condition = env.Eval(listArgs.Value[0]);
        if (condition is BooleanObj boolCondition)
        {
            return boolCondition.Value ? env.Eval(listArgs.Value[1]) : listArgs.Value.Count == 3 ? env.Eval(listArgs.Value[2]) : new SymbolObj("unspecified");
        }
        return new SymbolObj("unspecified");
    }

    public static Obj Lambda(Obj args, Env env)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2 || listArgs.Value[0] is not ListObj)
            throw new ArgumentException("lambda requires two arguments: parameters and body.");

        var parameters = (ListObj)listArgs.Value[0];
        var body = listArgs.Value[1];

        return new ClosureObj(parameters, body, env);
    }

    public static Obj Let(Obj args, Env env)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2 || listArgs.Value[0] is not ListObj bindingsList)
            throw new ArgumentException("let requires exactly 2 arguments, and the first must be a list of bindings.");

        var newEnv = new Env(env);
        foreach (var binding in bindingsList.Value)
        {
            if (!(binding is ListObj { Value: [SymbolObj, _] } singleBinding))
                throw new ArgumentException("Invalid binding format.");

            var name = ((SymbolObj)singleBinding.Value[0]).Value;
            var value = newEnv.Eval(singleBinding.Value[1]);
            newEnv.Define(name, value);
        }
        return newEnv.Eval(listArgs.Value[1]);
    }

    public static Obj Or(Obj args, Env env)
    {
        if (args is not ListObj listArgs)
            return new SymbolObj("unspecified");

        foreach (var arg in listArgs.Value)
        {
            var result = env.Eval(arg);
            if (result is BooleanObj { Value: true })
                return result;
        }
        return new SymbolObj("unspecified");
    }

    public static Obj Quote(Obj args, Env env)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 1)
            throw new ArgumentException("quote requires exactly 1 argument.");

        return listArgs.Value[0];
    }

    public static Obj Set(Obj args, Env env)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2 || listArgs.Value[0] is not SymbolObj)
            throw new ArgumentException("set! requires exactly 2 arguments, and the first must be a symbol.");

        var name = ((SymbolObj)listArgs.Value[0]).Value;
        var newValue = env.Eval(listArgs.Value[1]);
        env.Set(name, newValue);

        return new SymbolObj("ok");
    }

    public static Obj While(Obj args, Env env)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2)
            throw new ArgumentException("while requires exactly 2 arguments.");

        while (true)
        {
            var condition = env.Eval(listArgs.Value[0]);
            if (!(condition is BooleanObj boolCondition) || !boolCondition.Value)
                break;

            env.Eval(listArgs.Value[1]);
        }
        return new SymbolObj("ok");
    }
}