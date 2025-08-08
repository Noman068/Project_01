using System;
using System.Collections.Generic;

public class CommandManager : ICommandManager
{
    private readonly Stack<CommandDealer> undoStack;
    private readonly Stack<CommandDealer> redoStack;
    private readonly int maxUndo;

    public CommandManager(int maxUndo = 10)
    {
        this.maxUndo = maxUndo;
        this.undoStack = new Stack<CommandDealer>();
        this.redoStack = new Stack<CommandDealer>();
    }

    public void ExecuteCommand(CommandDealer command)
    {
        undoStack.Push(command);
        if (undoStack.Count > maxUndo)
        {
            var tempStack = new Stack<CommandDealer>(undoStack.ToArray());
            undoStack.Clear();
            foreach (var cmd in tempStack)
            {
                undoStack.Push(cmd);
            }
        }
        redoStack.Clear();
    }

    public void Undo()
    {
        if (undoStack.Count == 0)
            return;

        CommandDealer command = undoStack.Pop();
        redoStack.Push(command);
    }

    public void Redo()
    {
        if (redoStack.Count == 0)
            return;

        CommandDealer command = redoStack.Pop();
        undoStack.Push(command);
    }

    public bool CanUndo()
    {
        return undoStack.Count > 0;
    }

    public bool CanRedo()
    {
        return redoStack.Count > 0;
    }

    public CommandDealer GetLastCommand()
    {
        return undoStack.Count > 0 ? undoStack.Peek() : null;
    }
} 