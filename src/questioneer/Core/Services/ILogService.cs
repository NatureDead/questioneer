using System;
using System.Threading.Tasks;
using Discord;

namespace questioneer.Core.Services
{
    public interface ILogService
    {
        Task LogAsync(LogMessage logMessage);

        void Log(Exception exception);
    }
}