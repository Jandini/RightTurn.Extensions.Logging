using Microsoft.Extensions.Logging;
using System;

namespace RightTurn.Extensions.Logging
{
    public static class TurnLoggingExtensions
    {
        public static ITurn WithLogging(this ITurn turn, Action<ILoggingBuilder> logging)
        {
            _ = turn.Directions.Add<ITurnLogging>(new TurnLogging(logging));
            return turn;
        }


        /// <summary>
        /// Use this extensions if you want to access IConfiguration when building logging. 
        /// The IConfiguration is build before logging so can be access through turn direction container.        
        /// </summary>
        /// <param name="turn">this Turn</param>
        /// <param name="loggingWithTurn">logging </param>
        /// <returns></returns>
        public static ITurn WithLogging(this ITurn turn, Action<ILoggingBuilder, ITurn> loggingWithTurn)
        {
            _ = turn.Directions.Add<ITurnLogging>(new TurnLogging((logging) => loggingWithTurn.Invoke(logging, turn)));
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
