using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Service.Cache;

namespace SMARTII.Service.Case.Strategy
{

    public sealed class CalculatorLogger
    {

        public TimeSpan? RemindHour { get; set; }

        public string Content { get; set; }

    }

    public sealed class PromiseDateTimeStrategy
    {
        private List<DayOfWeek> _workOffDays;
        private TimeSpanRange _workTimeSpanRange;
        private List<WorkSchedule> _workSchedule;


        public List<CalculatorLogger> _calcLogger = new List<CalculatorLogger>();


        public PromiseDateTimeStrategy() { }
        public PromiseDateTimeStrategy(HeaderQuarterTerm term)
        {
            InitializeTimeSpanRange(term.NodeKey);
            InitializeWorkOffDays(term.NodeKey);
            InitializeWorkSchedule(term.NodeID);

        }

        #region Initialize

        public void InitializeTimeSpanRange(string nodeKey)
        {
            if (!DataStorage.WorkTimeDict.TryGetValue(nodeKey, out var workTimeSpanRange))
                workTimeSpanRange = new TimeSpanRange(
                    new TimeSpan(0, 0, 0),
                    new TimeSpan(24, 59, 59));

            _workTimeSpanRange = workTimeSpanRange;

            _calcLogger.Add(new CalculatorLogger()
            {
                Content = $"初始化工作時段 , 開始時間 : {_workTimeSpanRange.Start.ToString()} ," +
                          $" 結束時間 : {_workTimeSpanRange.End.ToString()}",
            });
        }

        public void InitializeWorkOffDays(string nodeKey)
        {
            if (!DataStorage.WorkOffDayDict.TryGetValue(nodeKey, out var workOffDays))
                workOffDays = new List<DayOfWeek>();

            _workOffDays = workOffDays.ToList();

            _calcLogger.Add(new CalculatorLogger()
            {
                Content = $"初始化例假日日期清單 , {JsonConvert.SerializeObject(_workOffDays)}"
            });
        }

        public void InitializeWorkSchedule(int nodeID)
        {
            if (!DataStorage.WorkScheduleDict.TryGetValue(nodeID, out var workSchedule))
                workSchedule = new List<WorkSchedule>();

            _workSchedule = workSchedule.ToList();

            _calcLogger.Add(new CalculatorLogger()
            {
                Content = $"初始化特假日日期清單 , {JsonConvert.SerializeObject(_workSchedule)}"
            });
        }


        #endregion

        /// <summary>
        /// 計算進入點
        /// </summary>
        /// <param name="workHour"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public DateTime Calculator(int workHour, DateTime? dateTime = null)
        {

            dateTime = dateTime ?? DateTime.Now;

            var remainingTimeSpan = new TimeSpan(workHour, 0, 0);

            _calcLogger.Add(new CalculatorLogger()
            {
                RemindHour = remainingTimeSpan,
                Content = $"準備進行計算 , 預計工時 : {workHour.ToString()} , " +
                     $"開始時間 : {dateTime.Value.ToString("yyyy/MM/dd HH:mm:ss")}"
            });

            return CalcPromiseDateTimeRecursive(dateTime.Value, ref remainingTimeSpan);
        }

        /// <summary>
        /// 重新校正開始日期
        /// </summary>
        /// <param name="cursorDateTime"></param>
        /// <returns></returns>
        DateTime CorrectionCursorDateTime(DateTime cursorDateTime)
        {

            DateTime recursor = default(DateTime);

            var time = cursorDateTime.TimeOfDay;

            if (_workTimeSpanRange.Start <= time && _workTimeSpanRange.End >= time)
            {
                recursor = cursorDateTime;
            }
            else
            {
                cursorDateTime = cursorDateTime.AddDays(1);
                recursor = RecursorTime(cursorDateTime);
            }

            _calcLogger.Add(new CalculatorLogger()
            {
                Content = $"重新校正時間  : {recursor.ToString("yyyy/MM/dd HH:mm:ss")} "
            });

            return recursor;

        }

        /// <summary>
        /// 重新校正開始時間
        /// </summary>
        /// <param name="cursorDateTime"></param>
        /// <returns></returns>
        DateTime RecursorTime(DateTime cursorDateTime) => cursorDateTime.Date + _workTimeSpanRange.Start;

        /// <summary>
        /// 計算減數
        /// </summary>
        /// <param name="cursorDateTime"></param>
        /// <param name="remainingHour"></param>
        /// <returns></returns>
        TimeSpan ConsiderMinus(DateTime cursorDateTime, TimeSpan remainingHour)
        {
            // 上班截止時段
            var cutoffDateTime = cursorDateTime.Date + _workTimeSpanRange.End;
        
            var diff = cutoffDateTime - cursorDateTime;

            return diff > remainingHour ? remainingHour : diff;

        }

        /// <summary>
        /// 進行剩餘小時異動
        /// </summary>
        /// <param name="cursorDateTime"></param>
        /// <param name="remainingHour"></param>
        /// <returns></returns>
        DateTime ExecuteDiff(DateTime cursorDateTime, ref TimeSpan remainingHour)
        {

            _calcLogger.Add(new CalculatorLogger()
            {
                Content = $"準備進行剩餘小時的扣除" +
                          $"目前運算起始時間 : {cursorDateTime.ToString("yyyy/MM/dd HH:mm:ss")}",
                RemindHour = remainingHour

            });


            var actualDiff = ConsiderMinus(cursorDateTime, remainingHour);

            remainingHour -= actualDiff;

            if (remainingHour.Ticks <= 0)
            {
                cursorDateTime += actualDiff;

                _calcLogger.Add(new CalculatorLogger()
                {
                    Content = $"小時扣除完畢" +
                      $"無剩餘時數 , 運算結束 , 預計完成時間為  : {cursorDateTime.ToString("yyyy/MM/dd HH:mm:ss")}",
                    RemindHour = remainingHour

                });

                return cursorDateTime;
            }
            else
            {
                // 跨日重算
                cursorDateTime = cursorDateTime.AddDays(1);
                cursorDateTime = RecursorTime(cursorDateTime);

                _calcLogger.Add(new CalculatorLogger()
                {
                    Content = $"小時扣除完畢" +
                        $"下次運算起始時間 : {cursorDateTime.ToString("yyyy/MM/dd HH:mm:ss")}",
                    RemindHour = remainingHour

                });

                return CalcPromiseDateTimeRecursive(cursorDateTime, ref remainingHour);
            }




        }

        /// <summary>
        /// 取例假日 , 與特假日之交集
        /// 算出實際上下班日
        /// </summary>
        /// <param name="cursorDateTime"></param>
        /// <returns></returns>
        WorkType Intersection(DateTime cursorDateTime)
        {
            // 取得目前日期
            var date = cursorDateTime.Date;
            // 取得目前星期
            var dayofWeek = date.DayOfWeek;


            // 是否為例假日主檔 , 若有則依此為主
            var specific = _workSchedule.FirstOrDefault(x => x.Date == date);

            if (specific != null) return specific.WorkType;

            var dailyWorkoff = _workOffDays.Any(x => x == dayofWeek);

            _calcLogger.Add(new CalculatorLogger()
            {
                Content = $"進行是否休假日驗算" +
                          $"運算時間 : {cursorDateTime.ToString("yyyy/MM/dd HH:mm:ss")}" +
                          $"是否休假 : {(dailyWorkoff ? "是" : "否")}",

            });

            return dailyWorkoff ? WorkType.WorkOff : WorkType.WorkOn;

        }

        /// <summary>
        /// 上班算法
        /// </summary>
        /// <param name="cursorDateTime"></param>
        /// <param name="remainingHour"></param>
        /// <returns></returns>
        DateTime WhenWorkOn(DateTime cursorDateTime, ref TimeSpan remainingHour)
        {
            return ExecuteDiff(cursorDateTime, ref remainingHour);
        }

        /// <summary>
        /// 休假算法
        /// </summary>
        /// <param name="cursorDateTime"></param>
        /// <param name="remainingHour"></param>
        /// <returns></returns>
        DateTime WhenWorkOff(DateTime cursorDateTime, ref TimeSpan remainingHour)
        {
            // 初始時間設定 , 為明天上班日
            cursorDateTime = cursorDateTime.AddDays(1);
            cursorDateTime = RecursorTime(cursorDateTime);

            return CalcPromiseDateTimeRecursive(cursorDateTime, ref remainingHour);
        }

        /// 這邊執行內部函式
        /// 避免參數傳遞混亂
        /// <param name="cursorDateTime">指標日期</param>
        /// <param name="remainingHour">剩餘天數</param>
        /// 運用內部變數以下 : 
        ///   -> _workTimeSpanRange  (工作時段) 
        ///   -> _workDays           (企業休假日) 
        ///   -> _workSchedule       (企業特殊假日)
        DateTime CalcPromiseDateTimeRecursive(DateTime cursorDateTime, ref TimeSpan remainingHour)
        {

            // 先校正工作日以及時間
            cursorDateTime = CorrectionCursorDateTime(cursorDateTime);

            // 先交集 , 取得實際上班日
            var workType = Intersection(cursorDateTime);

            return workType == WorkType.WorkOn ?
                WhenWorkOn(cursorDateTime, ref remainingHour) :
                WhenWorkOff(cursorDateTime, ref remainingHour);

        }

    }
}
