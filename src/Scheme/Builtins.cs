namespace Scheme;

public static class BuiltinsX
{
    public static Obj Add(Obj args)
    {
        var sum = 0.0;
        
        if (args is not ListObj listArgs)
            return new NumberObj(sum);
        
        foreach (var arg in listArgs.Value)
        {
            if (arg is NumberObj numberObj)
            {
                sum += numberObj.Value;
            }
            else
            {
                throw new Exception("All arguments to + must be numbers.");
            }
        }
        
        return new NumberObj(sum);
    }
    
    public static Obj Car(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 1 || listArgs.Value[0] is not ListObj innerList)
            throw new Exception("car requires a single list argument.");

        if (innerList.Value.Count == 0)
            throw new Exception("Cannot take car of an empty list.");

        return innerList.Value[0];
    }
    
    public static Obj Cdr(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 1 || listArgs.Value[0] is not ListObj innerList)
            throw new Exception("cdr requires a single list argument.");

        if (innerList.Value.Count == 0)
            throw new Exception("Cannot take cdr of an empty list.");

        return new ListObj(innerList.Value.GetRange(1, innerList.Value.Count - 1));
    }
    
    public static Obj Cons(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2 || listArgs.Value[1] is not ListObj innerList)
            throw new Exception("cons requires two arguments, the second of which must be a list.");

        var newList = new List<Obj> { listArgs.Value[0] };
        newList.AddRange(innerList.Value);
        return new ListObj(newList);
    }
    
    public static Obj CreateList(Obj args)
    {
        if (args is not ListObj listArgs)
            throw new Exception("list requires a list of arguments.");

        return new ListObj(new List<Obj>(listArgs.Value));
    }
    
    public static Obj Display(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 1)
            throw new Exception("display requires exactly 1 argument.");

        var arg = listArgs.Value[0];
        Console.Write(arg.ToString());
        
        return new SymbolObj("ok");
    }
    
    public static Obj Div(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count == 0)
            throw new Exception("Division requires at least one argument.");

        if (listArgs.Value[0] is not NumberObj firstNumber)
            throw new Exception("All arguments to / must be numbers.");

        var quotient = firstNumber.Value;

        for (var i = 1; i < listArgs.Value.Count; i++)
        {
            if (listArgs.Value[i] is NumberObj numberObj)
            {
                if (numberObj.Value == 0)
                    throw new Exception("Division by zero.");
                    
                quotient /= numberObj.Value;
            }
            else
            {
                throw new Exception("All arguments to / must be numbers.");
            }
        }

        return new NumberObj(quotient);
    }
    
    public static Obj Equal(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count < 2)
            throw new Exception("= requires at least two arguments.");

        var first = listArgs.Value[0] as NumberObj ?? throw new Exception("All arguments to = must be numbers.");
            
        return new BooleanObj(listArgs.Value.Skip(1).All(arg => 
        {
            var num = arg as NumberObj ?? throw new Exception("All arguments to = must be numbers.");
            return first.Value == num.Value;
        }));
    }
    
    public static Obj Filter(Obj args)
    {
        if (args is not ListObj { Value: [_, ListObj] } listArgs)
            throw new Exception("filter requires a function and a list.");

        var fn = listArgs.Value[0];
        var list = (ListObj)listArgs.Value[1];

        var newList = new List<Obj>();

        foreach (var item in list.Value)
        {
            Obj result;

            switch (fn)
            {
                case FunctionObj functionObj:
                    result = functionObj.Func(new ListObj(new List<Obj> { item }));
                    break;
                case ClosureObj closureObj:
                {
                    var newEnv = new Env(closureObj.Env);
                    newEnv.Define(closureObj.Params.Value[0].ToString(), item);
                    result = newEnv.Eval(closureObj.Body);
                    break;
                }
                default:
                    throw new Exception("filter requires a function or closure as the first argument.");
            }

            if (result is BooleanObj booleanObj && booleanObj.Value)
            {
                newList.Add(item);
            }
        }

        return new ListObj(newList);
    }
    
    public static Obj GreaterThan(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2)
            throw new Exception("> requires exactly two arguments.");

        var first = listArgs.Value[0] as NumberObj ?? throw new Exception("All arguments to > must be numbers.");
        var second = listArgs.Value[1] as NumberObj ?? throw new Exception("All arguments to > must be numbers.");
            
        return new BooleanObj(first.Value > second.Value);
    }
    
    public static Obj GreaterThanOrEqual(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2)
            throw new Exception(">= requires exactly two arguments.");

        var first = listArgs.Value[0] as NumberObj ?? throw new Exception("All arguments to >= must be numbers.");
        var second = listArgs.Value[1] as NumberObj ?? throw new Exception("All arguments to >= must be numbers.");
            
        return new BooleanObj(first.Value >= second.Value);
    }
    
    public static Obj IsNull(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 1 || listArgs.Value[0] is not ListObj innerList)
            throw new Exception("null? requires a single list argument.");

        return new BooleanObj(innerList.Value.Count == 0);
    }
    
    public static Obj LessThan(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2)
            throw new Exception("< requires exactly two arguments.");

        var first = listArgs.Value[0] as NumberObj ?? throw new Exception("All arguments to < must be numbers.");
        var second = listArgs.Value[1] as NumberObj ?? throw new Exception("All arguments to < must be numbers.");
            
        return new BooleanObj(first.Value < second.Value);
    }
    
    public static Obj LessThanOrEqual(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 2)
            throw new Exception("<= requires exactly two arguments.");

        var first = listArgs.Value[0] as NumberObj ?? throw new Exception("All arguments to <= must be numbers.");
        var second = listArgs.Value[1] as NumberObj ?? throw new Exception("All arguments to <= must be numbers.");
            
        return new BooleanObj(first.Value <= second.Value);
    }
    
    public static Obj Map(Obj args)
    {
        if (args is not ListObj { Value: [_, ListObj] } listArgs)
            throw new Exception("map requires a function and a list.");

        var fn = listArgs.Value[0];
        var list = (ListObj)listArgs.Value[1];

        var newList = new List<Obj>();
        
        foreach (var item in list.Value)
        {
            Obj result;

            switch (fn)
            {
                case FunctionObj functionObj:
                    result = functionObj.Func(new ListObj(new List<Obj> { item }));
                    break;
                case ClosureObj closureObj:
                {
                    var newEnv = new Env(closureObj.Env);
                    newEnv.Define(closureObj.Params.Value[0].ToString(), item);
                    result = newEnv.Eval(closureObj.Body);
                    break;
                }
                default:
                    throw new Exception("map requires a function or closure as the first argument.");
            }

            newList.Add(result);
        }

        return new ListObj(newList);
    }
    
    public static Obj Mul(Obj args)
    {
        var product = 1.0;

        if (args is not ListObj listArgs)
            return new NumberObj(product);

        foreach (var arg in listArgs.Value)
        {
            if (arg is NumberObj numberObj)
            {
                product *= numberObj.Value;
            }
            else
            {
                throw new Exception("All arguments to * must be numbers.");
            }
        }

        return new NumberObj(product);
    }
    
    public static Obj Newline(Obj args)
    {
        if (args is ListObj listArgs && listArgs.Value.Count > 0)
            throw new Exception("newline does not take any arguments.");

        Console.WriteLine();
        return new SymbolObj("ok");
    }
    
    public static Obj Not(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count != 1)
            throw new Exception("Not requires exactly 1 argument.");
            
        var arg = listArgs.Value[0];

        if (arg is BooleanObj booleanObj)
        {
            return new BooleanObj(!booleanObj.Value);
        }

        throw new Exception("Argument to not must be a boolean.");
    }
    
    public static Obj Reduce(Obj args)
    {
        if (args is not ListObj { Value: [_, ListObj, not null] } listArgs)
            throw new Exception("reduce requires a function, a list, and an initial value.");

        var fn = listArgs.Value[0];
        var list = (ListObj)listArgs.Value[1];
        var accumulator = listArgs.Value[2];

        foreach (var item in list.Value)
        {
            Obj result;

            switch (fn)
            {
                case FunctionObj functionObj:
                    result = functionObj.Func(new ListObj(new List<Obj> { accumulator, item }));
                    break;
                case ClosureObj closureObj:
                {
                    var newEnv = new Env(closureObj.Env);
                    newEnv.Define(closureObj.Params.Value[0].ToString(), accumulator);
                    newEnv.Define(closureObj.Params.Value[1].ToString(), item);
                    result = newEnv.Eval(closureObj.Body);
                    break;
                }
                default:
                    throw new Exception("reduce requires a function or closure as the first argument.");
            }

            accumulator = result;
        }

        return accumulator;
    }
    
    public static Obj Sub(Obj args)
    {
        if (args is not ListObj listArgs || listArgs.Value.Count == 0)
            throw new Exception("Subtraction requires at least one argument.");

        if (listArgs.Value[0] is not NumberObj firstNumber)
            throw new Exception("All arguments to - must be numbers.");

        var difference = firstNumber.Value;

        for (var i = 1; i < listArgs.Value.Count; i++)
        {
            if (listArgs.Value[i] is NumberObj numberObj)
            {
                difference -= numberObj.Value;
            }
            else
            {
                throw new Exception("All arguments to - must be numbers.");
            }
        }

        return new NumberObj(difference);
    }
}