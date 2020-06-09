using Discord.Commands;
using System.Threading.Tasks;

namespace questioneer.Modules
{
    public class AmthorModule : ModuleBase<SocketCommandContext>
    {
        [Command("amthor")]
        [Summary("Just Amthor.")]
        public async Task AmthorAsync()
        {
            await ReplyAsync("Na, du alter Zertörer ~");
            await ReplyAsync("https://www.youtube.com/watch?v=4IKUlVe2t-g");
        }
    }
}