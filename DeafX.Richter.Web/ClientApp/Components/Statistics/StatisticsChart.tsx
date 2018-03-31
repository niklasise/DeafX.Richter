import * as React from "react";
import * as Chart from "chart.js";
import StatisticsTimeSpan from "../../Models/Statistics/StatisticsTimeSpan";

Chart.defaults.global.defaultFontColor = "#FFF";
Chart.defaults.global.defaultFontFamily = "Open Sans";

const chartColors: string[] = [
    "215, 219, 221",
    "233,30,99",
    "241, 196, 15",
    "230, 126, 34",
    "46, 204, 113",
    "142, 68, 173",
];

export interface ChartComponentProps
{
    id: string,
    data?: ChartComponentData,
    className?: string
    timeSpan: StatisticsTimeSpan
}

export interface ChartComponentData {
    dataSets: {
        label: string,
        data: { x: number, y: number }[]
    }[]
}

enum DateFormat {
    TimeOnly, 
    DateOnly,
    DateTime,
    HoursAndMinutes
}

class StatisticsChartComponent extends React.Component<ChartComponentProps, any> {

    constructor()
    {
        super();

        this.getDateStringFromTimestamp = this.getDateStringFromTimestamp.bind(this);
        this.getDateFormat = this.getDateFormat.bind(this);
        this.getStepSize = this.getStepSize.bind(this);
    }

    getDateFormat(timeSpan: StatisticsTimeSpan)
    {
        switch (timeSpan)
        {
            case StatisticsTimeSpan.Day:
                return DateFormat.HoursAndMinutes;
            default:
                return DateFormat.DateOnly;
        }
    }

    getStepSize(timeSpan: StatisticsTimeSpan)
    {
        switch (timeSpan) {
            case StatisticsTimeSpan.Day:
                return 60 * 60 * 2; // 2 hours
            case StatisticsTimeSpan.Week:
                return 60 * 60 * 12; // 12 hours
            case StatisticsTimeSpan.Month:
                return 60 * 60 * 24 * 2; // 2 days
            case StatisticsTimeSpan.Year:
                return 60 * 60 * 24 * 31; // 31 days
            default:
                return 60 * 60 * 24; // 24 hours
        }
    }

    getDateStringFromTimestamp(timestamp: number, dateFormat: DateFormat = DateFormat.DateTime): string {
        var date = new Date(timestamp * 1000);

        var year = date.getFullYear();
        var month = "0" + (date.getMonth() + 1);
        var day = "0" + date.getDate();

        var hours = "0" + date.getHours();
        var minutes = "0" + date.getMinutes();
        var seconds = "0" + date.getSeconds();

        var formatedTime = `${hours.substr(-2)}:${minutes.substr(-2)}:${seconds.substr(-2)}`;
        var formatedDate = `${year}-${month.substr(-2)}-${day.substr(-2)}`;

        switch (dateFormat)
        {
            case DateFormat.DateOnly:
                return formatedDate;
            case DateFormat.TimeOnly:
                return formatedTime;
            case DateFormat.HoursAndMinutes:
                return `${hours.substr(-2)}:${minutes.substr(-2)}`;
            default:
                return `${formatedDate} ${formatedTime}`;
        }
    }

    public componentDidMount() {

        let ctx = (document.getElementById(this.props.id) as HTMLCanvasElement).getContext('2d');
        let chartData : any[] = new Array;

        let maxXVal = -Infinity;
        let minXVal = Infinity;
        let maxYVal = -Infinity;
        let minYVal = Infinity;

        this.props.data.dataSets.forEach((dataSet) => {
            dataSet.data.forEach((dataPoint) => {
                maxXVal = Math.max(maxXVal, dataPoint.x);
                minXVal = Math.min(minXVal, dataPoint.x);
                maxYVal = Math.max(maxYVal, dataPoint.y);
                minYVal = Math.min(minYVal, dataPoint.y);
            })
        })
        
        this.props.data.dataSets.forEach((o, i) => {
            chartData.push({
                ...o,
                fill: true,
                backgroundColor: `rgba(${chartColors[i % chartColors.length]},0.2)`,
                borderColor: `rgb(${chartColors[i % chartColors.length]})`,
                showLine: true
            });
        })

        let myLineChart = new Chart.Chart(this.props.id, {
            type: 'scatter',
            data: {
                datasets: chartData
            },
            options: {
                tooltips: {
                    callbacks: {
                        label: (item, data) =>
                        {
                            let dateTime = this.getDateStringFromTimestamp((data.datasets[item.datasetIndex].data[item.index] as any).x); 
                            let value = (data.datasets[item.datasetIndex].data[item.index] as any).y

                            return `${dateTime}: ${value}`;
                        }
                    }
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: "rgba(255, 255, 255, 0.3)", 
                        },
                        ticks: {
                            callback: (value, index, values) => {
                                return this.getDateStringFromTimestamp(value, this.getDateFormat(this.props.timeSpan));
                            },
                            min: minXVal,
                            max: maxXVal,
                            stepSize: this.getStepSize(this.props.timeSpan)
                        } as any,
                    }],
                    yAxes: [{
                        gridLines: {
                            color: "rgba(255, 255, 255, 0.3)",
                        },
                        ticks: {
                            min: minYVal > 0 ? 0 : minYVal,
                            max: maxYVal * 1.2
                        }
                    }]
                },
                responsive: true
            }

        });
    }

    public render() {
        return <div className={this.props.className} >
            <canvas id={this.props.id}></canvas>
        </div>;
    }
}

export default StatisticsChartComponent;

//[{
//    label: "Gästrum - Fönster",
//    data: [
//        21,
//        21.5,
//        23,
//        19,
//        null,
//        24,
//    ],
//    fill: true,
//    spanGaps: true
//},
//{
//    label: "Kök - Fönster",
//    data: [
//        19,
//        20.5,
//        -25,
//        18,
//        23,
//        22,
//    ],
//    fill: true,
//    spanGaps: true
//}]