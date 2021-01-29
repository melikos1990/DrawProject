export class DailyReportViewModel {
    NodeID: string;
    DateTimeRange: string;
    ReportType: string;
    ReportItems = [
        { id: 0, text: '客服報表' },
        { id: 1, text: '來電紀錄(時效)' },
    ];
}