using ReactiveUI;

namespace MarkDownAvalonia.ReactiveModels
{
    public class MarkdownTextRM : ReactiveObject
    {
        private string markdownTxt;

        public string MarkdownTxt
        {
            get => this.markdownTxt;
            set => this.RaiseAndSetIfChanged(ref markdownTxt, value);
        }
    }
}