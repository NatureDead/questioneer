namespace questioneer.Core.Entities
{
    public class MessagesFile : YamlFile
    {
        private const string NoKeyFoundMessage = "This message appears if the messages.yml was not fully configured.";

        protected override int NewestVersion => 0;
        protected override string ResourceName => "messages.yml";

        public string QuestionStarted(string questionName)
        {
            return GetMessage("question_started", questionName);
        }

        public string QuestionStopped(string questionName)
        {
            return GetMessage("question_stopped", questionName);
        }

        public string NoAnswer()
        {
            return GetMessage("no_answer");
        }

        public string AnswerFromTeam(string teamName, string answerMessage)
        {
            return GetMessage("answer_from_team", teamName, answerMessage);
        }

        private string GetMessage(string key, params object[] args)
        {
            var message = Configuration[key];
            if (string.IsNullOrWhiteSpace(message))
                return NoKeyFoundMessage;

            return string.Format(message, args);
        }
    }
}