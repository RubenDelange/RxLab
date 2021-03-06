﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using RxLab.Helpers;
using RxLab.Models;
using Tx.Windows;

namespace RxLab.Commands
{
    public static class DefaultCommands
    {
        #region private methods

        private static string ExecuteCommand(Func<string> command)
        {
            var resultAsString = command();

            if (string.IsNullOrEmpty(resultAsString))
            {
                return "Done!";
            }
            else
            {
                return resultAsString;
            }
        }

        /*
         * No need to create params as input parameters for the Func
         * It's possible to use the input parameters from the initial method in the Func operation
         * 
        private static string ExecuteCommand(Func<object[], string> command, params object[] parameters)
        {
            var resultAsString = command(parameters);

            if (string.IsNullOrEmpty(resultAsString))
            {
                return "Done!";
            }
            else
            {
                return resultAsString;
            }
        }
         */

        private static string ExecuteVoidCommand(Action voidCommand)
        {
            voidCommand();

            return "Done!";
        }

        #endregion

        public static string Pow()
        {
            return ExecuteVoidCommand(() =>
            {
                var timeInSecondsStream = Observable.Interval(TimeSpan.FromSeconds(1));

                var squaredTimeStream = timeInSecondsStream.Select(value => Math.Pow(value, 2));

                using (squaredTimeStream.Subscribe(Console.WriteLine))
                {
                    Console.WriteLine("Press any key to unsubscribe");
                    Console.ReadKey();
                }
            });
        }

        public static string Time()
        {
            return ExecuteVoidCommand(() =>
            {
                var timeStampedSecondsStream = Observable.Interval(TimeSpan.FromSeconds(1))
                                                     .Timestamp();

                using (timeStampedSecondsStream.Subscribe(x => Console.WriteLine("{0}: {1}", x.Value, x.Timestamp)))
                {
                    Console.WriteLine("Press any key to unsubscribe");
                    Console.ReadKey();
                }
            });
        }

        public static string EventLogWithTx()
        {
            //http://blogs.endjin.com/2014/05/event-stream-manipulation-using-rx-part-2/

            return ExecuteVoidCommand(() =>
            {
                //NuGet package Tx to read from Windows Performance counters
                var perfCounterStream = PerfCounterObservable.FromRealTime(TimeSpan.FromSeconds(1),
                                                                            new[]
                                                                        {
                                                                            @"\Processor(_Total)\% Processor Time",
                                                                            @"\Memory(_Total)\% Committed Bytes In Use",
                                                                            @"\Memory(_Total)\Available MBytes"
                                                                        })
                                                             .Buffer(TimeSpan.FromSeconds(1));

                perfCounterStream.Subscribe();

                //NuGet package Tx to read from Windows Event Log
                var eventStream = EvtxObservable.FromLog("TestLog")
                                                .Merge(EvtxObservable.FromLog("TestLog2"))
                                                .Select(eventRecord => new Event() { Record = eventRecord })
                                                .ComposeLatest(perfCounterStream,
                                                                (evt, perfSamples) =>
                                                                {
                                                                    evt.PerformanceSamples.AddRange(perfSamples);
                                                                    return evt;
                                                                })
                                                .Buffer(TimeSpan.FromSeconds(2));

                using (eventStream.Subscribe(new EventObserver()))
                {
                    Console.WriteLine("Press any key to unsubscribe");
                    Console.ReadKey();
                }
            });
        }
    }
}
