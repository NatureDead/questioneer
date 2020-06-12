using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace questioneer.Core.Modules
{
    public class AmthorModule : ModuleBase<SocketCommandContext>
    {
        [Command("amthor")]
        public Task AmthorAsync()
        {
            return PostMessage("Hey Rezo, du alter Zerstörer!",
                "https://www.youtube.com/watch?v=4IKUlVe2t-g");
        }

        [Command("spahn")]
        public Task SpahnAsync()
        {
            return PostMessage("Hört auf die Wissenschaftler! Hört auf die Fakten!",
                "https://www.youtube.com/watch?v=40MJUEQ6RJM");
        }

        private static IUserMessage _text;
        private static IUserMessage _link;

        private async Task PostMessage(string text, string link)
        {
            if (_text == null)
            {
                _text = await ReplyAsync(text).ConfigureAwait(false);
                _link = await ReplyAsync(link).ConfigureAwait(false);
            }
            else
            {
                await _text.ModifyAsync(x => x.Content = text).ConfigureAwait(false);
                await _link.ModifyAsync(x => x.Content = link).ConfigureAwait(false);
            }
        }
    }
}