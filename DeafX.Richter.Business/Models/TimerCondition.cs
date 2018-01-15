using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Common.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Models
{
    public class TimerCondition : IToggleAutomationCondition
    {
        public bool State { get; private set; }

        public event OnToggleAutomationConditionStateChangedHandler OnStateChanged;

        private List<TimerConditionInterval> _intervals;
        private TimerConditionInterval _currentInterval;
        private ILogger<TimerCondition> _logger;

        public TimerCondition(TimerConditionInterval[] intervals)
        {
            _logger = LoggerFactoryWrapper.CreateLogger<TimerCondition>();

            if(intervals == null || intervals.Length == 0)
            {
                throw new ArgumentException("Intervals cant be null or empty");
            }

            ValidateNoOverlappingConditions(intervals);

            // Order the intervals
            _intervals = intervals.OrderBy(i => i).ToList();

            Start();
        }

        private void Start()
        {
            var timeOfDay = DateTime.Now.TimeOfDay;

            var startInterval = _intervals.FirstOrDefault(i => (i.Start <= timeOfDay && i.End >= timeOfDay) || i.Start > timeOfDay);

            var intervalIndex = startInterval == null ? 0 : _intervals.IndexOf(startInterval);

            var interval = _intervals[intervalIndex];

            SetCurrentInterval(intervalIndex);

            CalculateState();

            SetTimer(intervalIndex, timeOfDay < interval.Start || timeOfDay >= interval.End);
        }

        private async void SetTimer(int intervalIndex,bool isStart)
        {
            var timeOfDay = DateTime.Now.TimeOfDay;

            _logger.LogDebug($"Setting Timer. TimeOfDay: {timeOfDay}. intervalIndex: {intervalIndex}. isStart: {isStart}");
            _logger.LogDebug($"_intervals[intervalIndex].Start: {_intervals[intervalIndex].Start}");
            _logger.LogDebug($"_intervals[intervalIndex].End: {_intervals[intervalIndex].End}");

            // Set triggertime to either interval start or end
            var triggerTime = isStart ? _intervals[intervalIndex].Start : _intervals[intervalIndex].End;

            _logger.LogDebug($"Trigger time set to: {triggerTime}");

            // Calculate time left unitl either Start or End of current interval
            var timeLeft = timeOfDay >= triggerTime ? triggerTime + TimeSpan.FromHours(24) - timeOfDay : triggerTime - timeOfDay;

            // Increment TimeLeft with 300ms in order to make sure that threa inst delayed to short due to the lack of precision on Task.Delay
            timeLeft += TimeSpan.FromMilliseconds(300);

            _logger.LogDebug($"TimeLeft: {timeLeft}. ActualTime: {DateTime.Now.TimeOfDay}");

            // Delay execution with calculated amount of time left
            await Task.Delay(timeLeft);

            _logger.LogDebug($"Delay complete. ActualTime: {DateTime.Now.TimeOfDay}");

            // Increment index if end-timer
            var nextIndex = isStart ? intervalIndex : intervalIndex + 1 < _intervals.Count ? intervalIndex + 1 : 0;

            // Update currentInterval if index change
            if (intervalIndex != nextIndex)
            {
                SetCurrentInterval(nextIndex);
            }

            CalculateState();

            SetTimer(nextIndex, !isStart);
        }

        private void SetCurrentInterval(int index)
        {
            _logger.LogDebug($"Setting current interval to: {index}");

            // Detach event handlers from additional conditions
            if(_currentInterval != null && _currentInterval.AdditionalConditions != null)
            {
                foreach(var condition in _currentInterval.AdditionalConditions)
                {
                    condition.OnStateChanged -= OnConditionStateChanged;
                }
            }

            _currentInterval = _intervals[index];

            // Hook up event handler to addional conditions
            if (_currentInterval.AdditionalConditions != null)
            {
                foreach (var condition in _currentInterval.AdditionalConditions)
                {
                    condition.OnStateChanged += OnConditionStateChanged;
                }
            }
        }

        private void OnConditionStateChanged(object sender, ToggleAutomationConditionStateChangedHandler args)
        {
            _logger.LogDebug($"Condition '{sender}' state changed to {args.NewState}");
            CalculateState();
        }

        private void CalculateState()
        {
            var timeOfDay = DateTime.Now.TimeOfDay;

            _logger.LogDebug($"Calculating state at time: {timeOfDay}");
            _logger.LogDebug($"_currentInterval.Start: {_currentInterval.Start}");
            _logger.LogDebug($"_currentInterval.End: {_currentInterval.End}");

            if (_currentInterval.AdditionalConditions != null)
            {
                foreach (var condition in _currentInterval.AdditionalConditions)
                {
                    _logger.LogDebug($"Additional Condition: {condition.State}");
                }
            }

            var newState =  timeOfDay >= _currentInterval.Start && 
                            timeOfDay < _currentInterval.End &&
                            (_currentInterval.AdditionalConditions == null ||
                            _currentInterval.AdditionalConditions.All(c => c.State));

            _logger.LogDebug($"newState: {newState}");

            if (newState != State)
            {
                _logger.LogDebug($"State changed, setting new state at RealTime - {DateTime.Now.TimeOfDay}");
                State = newState;
                OnStateChanged?.Invoke(this, new ToggleAutomationConditionStateChangedHandler(this, newState));
            }
        }

        private void ValidateNoOverlappingConditions(TimerConditionInterval[] intervals)
        {
            foreach(var interval in intervals)
            {
                if(intervals.Any(i => i != interval && interval.Start >= i.Start && interval.Start <= i.End))
                {
                    throw new ArgumentException("Intervals cannot overlap eachother");
                }
            }
        }
    }
}
