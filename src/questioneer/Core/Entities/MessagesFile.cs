using System;

namespace questioneer.Core.Entities
{
    public class MessagesFile : YamlFile
    {
        protected override int NewestVersion => 0;
        protected override string ResourceName => "messages.yml";

        public string QuestionStarted(string questionName)
        {
            return string.Format(Configuration["question_started"], questionName);
        }

        public string QuestionStopped(string questionName)
        {
            try
            {
                var xx = Configuration["question_stopped"];
            }
            catch (Exception exception)
            {
            }

            return string.Format(Configuration["question_stopped"], questionName);
        }
    }
}