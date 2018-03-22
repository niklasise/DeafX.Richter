import * as React from "react";
import * as Chart from "chart.js";

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
}

export interface ChartComponentData {
    //labels: string[],
    dataSets: {
        label: string,
        //data: number[]
        data: { x: number, y: number }[]
    }[]
}

class ChartComponent extends React.Component<ChartComponentProps, any> {

    public componentDidMount() {

        let ctx = (document.getElementById(this.props.id) as HTMLCanvasElement).getContext('2d');

        let chartData : any[] = new Array;

        this.props.data.dataSets.forEach((o, i) => {
            chartData.push({
                ...o,
                fill: true,
                //spanGaps: true,
                backgroundColor: `rgba(${chartColors[i % chartColors.length]},0.2)`,
                borderColor: `rgb(${chartColors[i % chartColors.length]})`,
                showLine: true
            });
        })

        let myLineChart = new Chart.Chart(this.props.id, {
            type: 'scatter',
            data: {
                //labels: this.props.data.labels,
                datasets: chartData
            },
            options: {
                tooltips: {
                    callbacks: {
                        label: (item, data) =>
                        {
                            return "Hej" + data.datasets[item.datasetIndex].data[item.index].y
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
                                return "Val:" + value;
                            }
                        },
                    }],
                    yAxes: [{
                        gridLines: {
                            color: "rgba(255, 255, 255, 0.3)",
                        }
                    }]
                },
                
            }

        });
    }

    public render() {
        return <div className={this.props.className} >
            <canvas id={this.props.id}></canvas>
        </div>;
    }
}

export default ChartComponent;

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