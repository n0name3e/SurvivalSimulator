namespace NeonEscape
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}
