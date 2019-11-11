using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    public class Logger
    {
        public string LastMessage { get; set; }

        public event EventHandler<DateTime> MessageLogged;

        public void Log(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException();

            LastMessage = message;

            // Write the log to a storage
            // ...

            MessageLogged?.Invoke(this, DateTime.UtcNow);
        }
    }
}
