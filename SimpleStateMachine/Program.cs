using System;
using Appccelerate.StateMachine.Machine;


//Naive();
//FluentApi();
Programmed("prprwps");


void Naive()
{
    var state = States.Available;
    Console.WriteLine($"Current state: {States.Available}");
    var finished = false;

    while (! finished)
    {
        var cmd = Console.ReadLine();
        if (cmd == "") { finished = true; continue; }

        switch (state)
        {
            case States.Available:
                if (cmd == "p")
                {
                    Console.WriteLine($"Current state: {States.InCart}");
                    state = States.InCart;
                }
                else if (cmd == "w")
                {
                    Console.WriteLine("Added to wish list.");
                }
                break;

            case States.InCart:
                if (cmd == "r")
                {
                    Console.WriteLine($"Current state: {States.Available}");
                    state = States.Available;
                }
                else if (cmd == "s")
                {
                    Console.WriteLine($"Current state: {States.Sold}");
                    state = States.Sold;
                }
                break;
        }
    }
}


void FluentApi()
{
    var builder = new StateMachineDefinitionBuilder<States, Events>();
    builder.In(States.Available).ExecuteOnEntry(() => Console.WriteLine($"Current state: {States.Available}"))
           .On(Events.PutToCart).Goto(States.InCart);
    builder.In(States.InCart).ExecuteOnEntry(() => Console.WriteLine($"Current state: {States.InCart}"))
           .On(Events.RemovedFromCart).Goto(States.Available)
           .On(Events.Paid).Goto(States.Sold);
    builder.In(States.Sold).ExecuteOnEntry(() => Console.WriteLine($"Current state: {States.Sold}"));
    builder.In(States.Available)
           .On(Events.AddedToWishList).Execute(() => Console.WriteLine("Added to wish list."));

    var sm = builder.WithInitialState(States.Available).Build().CreatePassiveStateMachine();
    sm.Start();

    while (sm.IsRunning)
    {
        var cmd = Console.ReadLine();
        switch (cmd)
        {
            case "": sm.Stop(); break;
            case "p": sm.Fire(Events.PutToCart); break;
            case "r": sm.Fire(Events.RemovedFromCart); break;
            case "w": sm.Fire(Events.AddedToWishList); break;
            case "s": sm.Fire(Events.Paid); break;
            default: Console.WriteLine("Commands: p, r, w, s."); break;
        }
    }
}


void Programmed(string program)
{
    var builder = new StateMachineDefinitionBuilder<States, Events>();
    builder.In(States.Available).ExecuteOnEntry(() => Console.WriteLine($"Current state: {States.Available}"))
           .On(Events.PutToCart).Goto(States.InCart);
    builder.In(States.InCart).ExecuteOnEntry(() => Console.WriteLine($"Current state: {States.InCart}"))
           .On(Events.RemovedFromCart).Goto(States.Available)
           .On(Events.Paid).Goto(States.Sold);
    builder.In(States.Sold).ExecuteOnEntry(() => Console.WriteLine($"Current state: {States.Sold}"));
    builder.In(States.Available)
           .On(Events.AddedToWishList).Execute(() => Console.WriteLine("Added to wish list."));

    var sm = builder.WithInitialState(States.Available).Build().CreatePassiveStateMachine();
    sm.Start();

    foreach (var cmd in program)
    {
        switch (cmd)
        {
            case 'p': sm.Fire(Events.PutToCart); break;
            case 'r': sm.Fire(Events.RemovedFromCart); break;
            case 'w': sm.Fire(Events.AddedToWishList); break;
            case 's': sm.Fire(Events.Paid); break;
            default: Console.WriteLine("Commands: p, r, w, s."); break;
        }
    }

    sm.Stop();
}
