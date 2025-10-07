namespace AIHelper.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public ICommand ListModels { get; protected set; }
        public ICommand ClearChat { get; protected set; }

        public ViewModelBase()
        {
            ListModels = ReactiveCommand.CreateFromTask(async () => { });
            ClearChat = ReactiveCommand.Create(() => { });
        }
    }
}