using System;
using System.Collections.Generic;
using UnityEngine;

public class TimedCallback
{
    public static void EmptyCallback() { }

    public Action action;
    public Action TickingAction;
    public float delay;
    public float RemDelay;
    public bool executed = false;
    public bool enabled = false;
    public string name = "";

    public TimedCallback(Action action, float delay, string name = "")
    {
        this.action = action;
        this.delay = delay;
        RemDelay = delay;
        this.name = name;
    }
    public TimedCallback(Action action, Action TickingAction, float delay, string name = "")
    {
        this.action = action;
        this.delay = delay;
        this.TickingAction = TickingAction;
        RemDelay = delay;
        this.name = name;
    }

    public void Tick()
    {
        if (executed) return;
        if (!enabled) return;
        RemDelay -= Time.deltaTime;
        if (RemDelay <= 0) { Execute(); RemDelay = 0; }
        else if (TickingAction != null) TickingAction.Invoke();
    }

    public void Execute()
    {
        if (action != null)
        action.Invoke();
        Stop();
        executed = true;
    }

    public void Start()
    {
        executed = false;
        enabled = true;
        RemDelay = delay;
    }

    public void Stop()
    {
        enabled = false;
        RemDelay = delay;
        executed = false;
    }

}

public class TimedCallbackSequence
{
    public List<TimedCallback> sequence = new List<TimedCallback>();
    public bool enabled = false;
    public int index = 0;

    public TimedCallbackSequence(List<TimedCallback> sequence)
    {
        this.sequence = sequence;
    }

    public TimedCallbackSequence()
    {

    }

    public void AddElement(Action action, float delay, string name = "")
    {
        sequence.Add(new TimedCallback(action, delay, name));
    }
    public void AddElement(Action action, Action TickingAction, float delay, string name = "")
    {
        sequence.Add(new TimedCallback(action, TickingAction, delay, name));
    }

    public void GoTo(string name)
    {
        foreach (var item in sequence)
        {
            if (item.name == name)
            {
                GoTo(sequence.IndexOf(item));
                return;
            }
        }
    }

    public void GoTo(int _index)
    {
        if (index == _index) return;
        if (!enabled) return;
        if (!sequence[index].executed)
            sequence[index].Execute();
        if (_index + 1 >= sequence.Count) { return; }
        index = _index;
        sequence[index].Start();
    }

    public int GetTimeLeftOfCurrentEvent()
    {
        return (int)Math.Round(sequence[index].RemDelay);
    }
    public TimedCallback GetCurrentEvent()
    {
        return (sequence[index]);
    }

    public void Next()
    {
        if (!enabled) return;
        if (!sequence[index].executed)
            sequence[index].Execute();
        if (index + 1 >= sequence.Count) { return; }
        index++;
        sequence[index].Start();
    }

    public void Tick()
    {
        if (!enabled) return;
        foreach (var item in sequence)
        {
            item.Tick();
        }
        if (sequence[index].executed)
        {
            if (index + 1 >= sequence.Count) { return; }
            index++;
            sequence[index].Start();
        }
    }

    public void Start()
    {
        enabled = true;
        index = 0;
        foreach (var item in sequence)
        {
            item.Stop();
        }
        sequence[0].Start();
    }

    public void Stop()
    {
        enabled = false;
        foreach (var item in sequence)
        {
            item.Stop();
        }
    }

}

public class TimedAbility
{
    public Action action;
    public float cooldown;
    public float RemCooldown;
    public bool enabled = false;
    public bool Infinite = true;
    public int UsesLeft = 0;
    public int Uses = 0;

    public static List<TimedAbility> abilities = new List<TimedAbility>();

    public bool Ready
    {
        get { return RemCooldown <= 0; }
    }

    public TimedAbility(Action action, float cooldown)
    {
        RemCooldown = cooldown;
        this.cooldown = cooldown;
        abilities.Add(this);
    }

    public void Start()
    {
        RemCooldown = cooldown;
        enabled = true;
    }

    public void Reset()
    {
        RemCooldown = cooldown;
        enabled = true;
    }

    public void ResetUses()
    {
        UsesLeft = Uses;
    }

    public void Stop()
    {
        enabled = false;
    }

    public void Tick()
    {
        if (!enabled) return;
        if (RemCooldown > 0)
        {
            RemCooldown -= Time.deltaTime;
        }
        else
        {
            RemCooldown = 0;
        }
    }

    public bool Use()
    {
        if (RemCooldown <= 0)
        {
            if (Infinite || UsesLeft > 0)
            {
                Reset();
                if (action != null)
                    action.Invoke();
                if (!Infinite)
                {
                    UsesLeft -= 1;
                }
                return true;
            }
        }
        return false;
    }

    public void SetUses(int uses)
    {
        UsesLeft = uses;
        Uses = uses;
        Infinite = false;
    }

    public static void ResetAllAbilities()
    {
        foreach (var item in abilities)
        {
            if(item!=null)
            item.Reset();
        }
    }
}