import * as React from "react";
import * as Chart from "chart.js";

Chart.defaults.global.defaultFontColor = "#FFF";
Chart.defaults.global.defaultFontFamily = "Open Sans";


export interface ChartComponentProps
{
    id: string,
    data?: any,

}

class ChartComponent extends React.Component<ChartComponentProps, any> {

    public componentDidMount() {

        var ctx = (document.getElementById(this.props.id) as HTMLCanvasElement).getContext('2d');

        var myLineChart = new Chart.Chart(this.props.id, {
            type: 'line',
            data: {
                labels: ["2017-01-01", "2017-01-02", "2017-01-03", "2017-01-04", "2017-01-05", "2017-01-06"],
                datasets: [{
                    label: "Gästrum - Fönster",
                    data: [
                        21,
                        21.5,
                        23,
                        19,
                        null,
                        24,
                    ],
                    fill: true,
                    spanGaps: true
                },
                {
                    label: "Kök - Fönster",
                    data: [
                        19,
                        20.5,
                        -25,
                        18,
                        23,
                        22,
                    ],
                    fill: true,
                    spanGaps: true
                }]
            },
            options: {            
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: "rgba(255, 255, 255, 0.3)", 
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
        return <div className="chartContainer" >
            <canvas id={this.props.id}></canvas>
        </div>;
    }
}

export default ChartComponent;