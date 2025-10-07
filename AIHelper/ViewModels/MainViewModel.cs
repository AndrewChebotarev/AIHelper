namespace AIHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private OllamaApiClient ollama;
        private Chat chat;

        private string prompt = "";
        private string generatedText = "";

        private bool isGenerating = false;

        public string Prompt
        {
            get => prompt;
            set => this.RaiseAndSetIfChanged(ref prompt, value);
        }

        public string GeneratedText
        {
            get => generatedText;
            set => this.RaiseAndSetIfChanged(ref generatedText, value);
        }

        public bool IsGenerating
        {
            get => isGenerating;
            set => this.RaiseAndSetIfChanged(ref isGenerating, value);
        }

        public MainWindowViewModel() => InitializeOllama();

        private void InitializeOllama()
        {
            try
            {
                Uri uri = new Uri("http://localhost:11434");
                ollama = new OllamaApiClient(uri);
                ollama.SelectedModel = "phi3:mini";
                chat = new Chat(ollama);
            }
            catch (Exception ex)
            {
                GeneratedText = $"❌ Ошибка инициализации: {ex.Message}";
            }
        }

        public async Task GenerateCode()
        {
            if (string.IsNullOrWhiteSpace(Prompt) || IsGenerating) return;

            IsGenerating = true;
            GeneratedText = "Генерирую...";

            try
            {
                GeneratedText = "";

                await foreach (var answerToken in chat.SendAsync(Prompt))
                    GeneratedText += answerToken;
            }
            catch (Exception ex)
            {
                GeneratedText = $"❌ Ошибка: {ex.Message}\n\nУбедитесь, что:\n1. Ollama запущен\n2. Модель phi3 скачана (ollama pull phi3)\n3. Ollama работает на localhost:11434";
            }
            finally
            {
                IsGenerating = false;
            }
        }
        public async Task ChangeModel(string modelName)
        {
            try
            {
                ollama.SelectedModel = modelName;
                GeneratedText = $"✅ Модель изменена на: {modelName}";
            }
            catch (Exception ex)
            {
                GeneratedText = $"❌ Ошибка смены модели: {ex.Message}";
            }
        }

        public async Task ListModels()
        {
            try
            {
                System.Collections.Generic.IEnumerable<OllamaSharp.Models.Model> models = await ollama.ListLocalModelsAsync();
                GeneratedText = "📋 Доступные модели:\n";

                foreach (var model in models)
                    GeneratedText += $"- {model.Name}\n";
            }
            catch (Exception ex)
            {
                GeneratedText = $"❌ Ошибка получения списка моделей: {ex.Message}";
            }
        }

        public void ClearChat()
        {
            chat = new Chat(ollama);
            GeneratedText = "💬 История чата очищена";
        }
    }
}