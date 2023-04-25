using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DL.DAL.Providers
{
    public class Utils
    {
        public const byte DEFAULT_MAX_RETRY = 5;

        #region Static helper methods

        #region Retry methods

        public static async Task<R> RunWithRetyAsync<R>(Func<Task<R>> task, Func<Exception, bool> doBreakOnFirstExceptionEvaluator = null, byte maxRetry = DEFAULT_MAX_RETRY)
        {
            byte attempt = 1;
            do
            {
                try
                {
                    return await task();
                }
                catch (Exception ex)
                {
                    if ((
                            attempt == 1
                            && doBreakOnFirstExceptionEvaluator != null
                            && doBreakOnFirstExceptionEvaluator(ex)
                        )
                        || attempt == maxRetry)
                    {
                        throw;
                    }
                }
                try
                {
                    await Task.Delay(new Random().Next(50, 200) * attempt);
                }
                catch { }
                attempt++;
            } while (attempt <= maxRetry);
            throw new Exception("Unable to process operation");
        }

        public static async Task RunWithRetyAsync(Func<Task> task, Func<Exception, bool> doBreakOnFirstExceptionEvaluator = null, byte maxRetry = DEFAULT_MAX_RETRY)
        {
            await RunWithRetyAsync(async () =>
            {
                await task();
                return true;
            }, doBreakOnFirstExceptionEvaluator, maxRetry);
        }

        #endregion Retry methods

        

        #endregion Static helper methods
    }
}