﻿using Microsoft.Extensions.Logging;
using System;

namespace RightTurn.Extensions.Logging
{
    public static class TurnLoggingExtensions
    {

        /// <summary>
        /// Add ITurnLogging to directions container. 
        /// This will trigger logging builder Action<ILoggingBuilder> if present in directions container.
        /// This extension is helper extenions for other extensions like serilog. 
        /// Existing WithLogging methods are calling AddLogging.
        /// If you build your own WithLogging remember to call AddLogging within. 
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static ITurn AddLogging(this ITurn turn)
        {
            turn.Directions.Add<ITurnLogging>(new TurnLogging());
            return turn;
        }


        public static ITurn WithLogging(this ITurn turn, Action<ILoggingBuilder> logging)
        {
            turn.Directions.Add(logging);
            turn.AddLogging();
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
            turn.Directions.Add<Action<ILoggingBuilder>>((logging) => loggingWithTurn.Invoke(logging, turn));
            turn.AddLogging();
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
