namespace questioneer.Core.Entities
{
    public class MessagesFile : YamlFile
    {
        protected override string ResourceName => "messages.yml";

        public string QuestionStarted { get; private set; }

        protected override void OnChanged()
        {
            QuestionStarted = Configuration["question_started"];

            base.OnChanged();
        }
    }
}