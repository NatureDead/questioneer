using System.Threading.Tasks;
using Discord.Commands;

namespace questioneer.Core.Modules
{
    public class AmthorModule : ModuleBase<SocketCommandContext>
    {
        [Command("amthor")]
        public Task AmthorAsync()
        {
            return PostMessage("Hey Rezo, du alter Zerstörer!",
                "https://youtu.be/4IKUlVe2t-g");
        }

        [Command("spahn")]
        public Task SpahnAsync()
        {
            return PostMessage("Also man kann heute als äh Elektroniker, als Mechatroniker äh als " +
                "als Zimmerer äh mehr verdienen als mancher Anwalt.",
                "https://youtu.be/40MJUEQ6RJM");
        }

        [Command("merkel")]
        public Task MerkelAsync()
        {
            return PostMessage("Jedem Anfang wohnt ein Zauber inne, der uns beschützt und der uns hilft zu leben.",
                "https://youtu.be/reCMB81A70k");
        }

        [Command("söder")]
        public Task SoederAsync()
        {
            return PostMessage("Der bayrische Weg war richtig.",
                "https://youtu.be/RQMdutoWHLM");
        }

        private async Task PostMessage(string text, string link)
        {
            await ReplyAsync(text).ConfigureAwait(false);
            await ReplyAsync(link).ConfigureAwait(false);
        }
    }
}