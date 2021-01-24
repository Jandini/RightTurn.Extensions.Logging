using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace RightTurn.Extensions.Logging
{
    internal class TurnLogging : ITurnLogging
    {
        public void AddLogging(ITurn turn)
        {
            if (turn.Directions.Have<Action<ILoggingBuilder>>(out var loggingBuilder))
                turn.Directions.Get<IServiceCollection>().AddLogging(loggingBuilder);
        }
    }
}
