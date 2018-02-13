import * as React from 'react';
import { match } from 'react-router-dom';
import { Device } from '../../Models/Device'
import Chart from './Chart';

//interface FieldState {
//    value: string,
//    error: string
//}

//interface LoginPageFields {
//    username: FieldState,
//    password: FieldState,
//}

//interface LoginPageState {
//    fields: LoginPageFields,
//    submitting: boolean,
//    validationErrors: string[]
//}

interface StatisticsPageParams {
    id: string
}

interface StatisticsPageProps {
    match?: match<StatisticsPageParams>
}

interface StatisticsPageState {
    device: Device,
    chartData: {
        dateTime: string,
        value: number
    }[]
}

class StatisticsPage extends React.Component<StatisticsPageProps, StatisticsPageState> {

    constructor()
    {
        super();

        this.state = {
            device: {
                id: "Device1",
                title: "Gästrum - Fönster",
                deviceType: "",
                isUpdating: false,
                lastChanged: null,
                value: "",
                valueType: ""
            },
            chartData: [
                {
                    dateTime: "2017-01-01",
                    value: 21.2
                },
                {
                    dateTime: "2017-01-02",
                    value: 20.2
                },
                {
                    dateTime: "2017-01-03",
                    value: 19
                },
                {
                    dateTime: "2017-01-04",
                    value: 22.5
                }
            ]
        }

    }

    public render() {
        return <div className="pageContainer">

            <div className="sectionContainer">

                <h1>{this.state.device.title}</h1>

                <Chart id="Chart1" />

                <select>
                    <option id="day">Senaste dagen</option>
                    <option id="week">Senaste veckan</option>
                    <option id="month">Senaste Månaden</option>
                    <option id="year">Senaste Året</option>
                </select>

            </div>

            
           {/* <ResponsiveContainer>
                {this.props.match.params.id}
                <LineChart >

                </LineChart>

            </ResponsiveContainer>
            */}
        </div>;
    }
}


export default StatisticsPage;