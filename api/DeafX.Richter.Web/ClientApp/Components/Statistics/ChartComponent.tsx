import * as React from "react";
import * as Chart from "chart.js";

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
                labels: ["2017-01-01", "2017-01-02", "2017-01-03", "2017-01-04", "2017-01-05"],
                datasets: [{
                    label: "Göstrum - Fönster",
                    data: [
                        21,
                        21.5,
                        23,
                        19,
                        20
                    ],
                    fill: true,
                }]
            }
        });
    }

    public render() {
        return <div className="chartContainer" >
            <canvas id={this.props.id} width={400} height={300}></canvas>
        </div>;
    }
}

export default ChartComponent;