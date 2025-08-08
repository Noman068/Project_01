public interface ICommandManager
{
    void ExecuteCommand(CommandDealer command);
    void Undo();
    void Redo();
    bool CanUndo();
    bool CanRedo();
    CommandDealer GetLastCommand();
} 