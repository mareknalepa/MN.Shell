using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MN.Shell.MVVM.Sample
{
    public class CommandsSampleViewModel : Screen
    {
        private int _counter;

        public int Counter
        {
            get => _counter;
            private set => Set(ref _counter, value);
        }

        public ICommand IncrementCounter { get; }


        private int _blockingCounter;

        public int BlockingCounter
        {
            get => _blockingCounter;
            private set => Set(ref _blockingCounter, value);
        }

        public ICommand IncrementBlockingCounter { get; }


        private int _asyncCounter;

        public int AsyncCounter
        {
            get => _asyncCounter;
            private set => Set(ref _asyncCounter, value);
        }

        public IAsyncCommand IncrementAsyncCounter { get; }

        public CommandsSampleViewModel()
        {
            Title = "Commands Sample";

            IncrementCounter = new RelayCommand(() => ++Counter);

            IncrementBlockingCounter = new RelayCommand(() =>
            {
                Thread.Sleep(3000);
                ++BlockingCounter;
            });

            IncrementAsyncCounter = new AsyncCommand(IncrementAsyncCounterAsync);
        }

        private async Task IncrementAsyncCounterAsync()
        {
            await Task.Delay(3000).ConfigureAwait(true);
            ++AsyncCounter;

            if (AsyncCounter % 3 == 1)
            {
                await Task.Delay(1000).ConfigureAwait(true);
                throw new ArgumentException("Example exception thrown out of async command");
            }
        }
    }
}
