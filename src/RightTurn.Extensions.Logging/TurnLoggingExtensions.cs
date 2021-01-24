using Microsoft.Extensions.Logging;
using System;

namespace RightTurn.Extensions.Logging
{
    public static class TurnLoggingExtensions
    {
        public static ITurn WithLogging(this ITurn turn)
        {
            turn.Directions.Add<ITurnLogging>(new TurnLogging());
            return turn;
        }

        public static ITurn WithLogging(this ITurn turn, Action<ILoggingBuilder> logging)
        {
            turn.Directions.Add(logging);
            return turn;
        }

        public static ITurn WithUnhandledExceptionLogging(this ITurn turn)
        {
            turn.Directions.Add<Func<Exception, ITurn, int>>((ex, turn) =>
            {
                if (!turn.Directions.HaveService<ILogger<Turn>>(out var logger))
                    throw ex;

                logger.LogCritical(ex.Message, ex);
                return ex.HResult;
            });

            return turn;
        }
    }
}
