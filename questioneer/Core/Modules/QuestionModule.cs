using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using questioneer.Core.Entities;
using questioneer.Core.Services;

namespace questioneer.Core.Modules
{
    public class QuestionModule : ModuleBase<SocketCommandContext>
    {
        public IServiceProvider Services { get; set; }

        [Command("question")]
        public Task QuestionAsync([Required] string name, [Required] ushort duration, [Optional] ushort overtime)
        {
            var question = new Question
            {
                Name = name,
                Duration = duration,
                Overtime = overtime
            };

            var questionService = Services.GetRequiredService<QuestionService>();
            return questionService.StartQuestionAsync(question);
        }
    }
}