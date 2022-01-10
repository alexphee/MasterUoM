using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public delegate void UpdateStackEvent();
public class ObservableStack<T> : Stack<T>
{
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;

    public ObservableStack(ObservableStack<T> items) : base(items) //give items and moving to the base so i can instantiate a stack with an amount of items
    {

    }

    public ObservableStack() //i need the empty constructor so i dont get an error when i instantiate an observable stack  without itesm
    {

    }
    public new void Push(T item) 
    {
        base.Push(item); //the observablestack inherits from stack so i need to call the base function to push item to the stack
        if(OnPush != null)
        {
            OnPush();
        }
    }

    public new T Pop()//return whatever i remove
    {
        T item = base.Pop();
        if(OnPop != null)
        {
            OnPop();
        }
        return item;
    }

    public new void Clear()
    {
        base.Clear();
        if(OnClear != null)
        {
            OnClear();
        }
    }
}
