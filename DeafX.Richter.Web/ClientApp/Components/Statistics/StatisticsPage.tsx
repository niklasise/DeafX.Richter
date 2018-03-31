import * as React from 'react';
import { match } from 'react-router-dom';
import { Device } from '../../Models/Device'
import Chart, { ChartComponentData } from './StatisticsChart';
import StatisticsTimeSpan from "../../Models/Statistics/StatisticsTimeSpan"
import StatisticApi from "../../Api/MockStatisticsApi";
//import StatisticsBadge from "./StatisticsBadge"
import styled from "styled-components";

interface StatisticsPageParams {
    id: string
}

interface StatisticsPageProps {
    match?: match<StatisticsPageParams>
}

interface StatisticsPageState {
    device: Device,
    chartData: ChartComponentData,
    selectedTimeSpan: StatisticsTimeSpan
}

//const StatisticsBadgeMargin = styled(StatisticsBadge) `
//    margin-bottom: 20px;
//`

const LeftSpan = styled.div`
    float: left;
    width: 200px;
`
const RightSpan = styled.div`
    float: left;
`

const OverflowDiv = styled.div`
    overflow: hidden;
    line-height: 42px;
    
`

class StatisticsPage extends React.Component<StatisticsPageProps, StatisticsPageState> {

    constructor()
    {
        super();

        this.state = {
            selectedTimeSpan: StatisticsTimeSpan.Day,
            device: {
                id: "Device1",
                title: "Gästrum - Fönster",
                deviceType: "",
                isUpdating: false,
                lastChanged: null,
                value: "",
                valueType: ""
            },
            chartData: {
                dataSets: [
                {
                    label: "kök - fönster",
                    data: [
                        { x: 1543626000 + 3600 * 0, y: 17 },
                        { x: 1543626000 + 3600 * 1, y: 21.5 },
                        { x: 1543626000 + 3600 * 2, y: 19 },
                        { x: 1543626000 + 3600 * 3, y: 17 },
                        { x: 1543626000 + 3600 * 4, y: 21 },
                        { x: 1543626000 + 3600 * 5, y: 24 },
                        { x: 1543626000 + 3600 * 6, y: 22 },
                        { x: 1543626000 + 3600 * 7, y: 21 },
                        { x: 1543626000 + 3600 * 8, y: 21.5 },
                        { x: 1543626000 + 3600 * 9, y: 19 },
                        { x: 1543626000 + 3600 * 10, y: 17 },
                        { x: 1543626000 + 3600 * 11, y: 21 },
                        { x: 1543626000 + 3600 * 12, y: 23 },
                        { x: 1543626000 + 3600 * 13, y: 24 },
                        { x: 1543626000 + 3600 * 14, y: 22 },
                        { x: 1543626000 + 3600 * 15, y: 21.5 },
                        { x: 1543626000 + 3600 * 16, y: 19 },
                        { x: 1543626000 + 3600 * 17, y: 17 },
                        { x: 1543626000 + 3600 * 18, y: 21 },
                        { x: 1543626000 + 3600 * 19, y: 24 },
                        { x: 1543626000 + 3600 * 20, y: 22 },
                        { x: 1543626000 + 3600 * 21, y: 21 },
                        { x: 1543626000 + 3600 * 22, y: 17 },
                        { x: 1543626000 + 3600 * 23, y: 15 },
                    ]
                    },
                    {
                        label: "kök - fönster",
                        data: [
                            { x: 1543625231 + 3600 * 0, y: 15 },
                            { x: 1543625231 + 3600 * 1, y: 20.5 },
                            { x: 1543625231 + 3600 * 2, y: 16 },
                            { x: 1543625231 + 3600 * 3, y: 16 },
                            { x: 1543625231 + 3600 * 4, y: 20.3 },
                            { x: 1543625231 + 3600 * 5, y: 22 },
                            { x: 1543625231 + 3600 * 6, y: 21.5 },
                            { x: 1543625231 + 3600 * 7, y: 20 },
                            { x: 1543625231 + 3600 * 8, y: 21 },
                            { x: 1543625231 + 3600 * 9, y: 18 },
                            { x: 1543625231 + 3600 * 10, y: 17 },
                            { x: 1543625231 + 3600 * 11, y: 19 },
                            { x: 1543625231 + 3600 * 12, y: 22 },
                            { x: 1543625231 + 3600 * 13, y: 23 },
                            { x: 1543625231 + 3600 * 14, y: 19 },
                            { x: 1543625231 + 3600 * 15, y: 21 },
                            { x: 1543625231 + 3600 * 16, y: 18.5 },
                            { x: 1543625231 + 3600 * 17, y: 16 },
                            { x: 1543625231 + 3600 * 18, y: 20 },
                            { x: 1543625231 + 3600 * 19, y: 23 },
                            { x: 1543625231 + 3600 * 20, y: 20 },
                            { x: 1543625231 + 3600 * 21, y: 19 },
                            { x: 1543625231 + 3600 * 22, y: 15 },
                            { x: 1543625231 + 3600 * 23, y: 13 },
                        ]
                    }]
            }
        }
    }

    private loadData() {

        var currentTimestamp = Math.floor(Date.now() / 1000);

        //StatisticApi.getStatistics(
        //    "abc123",
        //    currentTimestamp,
        //    this.getFromTime(currentTimestamp, this.state.selectedTimeSpan),
        //    this.getMinumimInterval(this.state.selectedTimeSpan)).then((data) => {
        //        this.setState({
        //            ...this.state,
        //            chartData: data
        //        }
        //    )
        //    })



    }

    private getFromTime(timeStamp: number, timeSpan: StatisticsTimeSpan) : number {
        let timeToSubtract = 0;

        switch (timeSpan) {
            case StatisticsTimeSpan.Day:
                timeToSubtract = 60 * 60 * 24;
                break;
            case StatisticsTimeSpan.Week:
                timeToSubtract = 60 * 60 * 24 * 7;
                break;
            case StatisticsTimeSpan.Month:
                timeToSubtract = 60 * 60 * 24 * 31;
                break;
            case StatisticsTimeSpan.Year:
                timeToSubtract = 60 * 60 * 24 * 365;
                break;
        }

        return timeStamp - timeToSubtract;
    }

    private getMinumimInterval(timeSpan: StatisticsTimeSpan) : number {
        switch (timeSpan) {
            case StatisticsTimeSpan.Day:
                return 60 * 15; // 15 minutes
            case StatisticsTimeSpan.Week:
                return 60 * 60 * 6; // 6 hours
            case StatisticsTimeSpan.Month:
                return 60 * 60 * 12; // 12 hours
            case StatisticsTimeSpan.Year:
                return 60 * 60 * 24 * 7; // 7 days
            default:
                return 60 * 60 * 24; // 24 hours
        }
    }


    public componentWillMount() {
        //loadData();
    }

    public render() {
        return <div className="pageContainer">

            <div className="sectionContainer mb20">

                <h1>{this.state.device.title}</h1>

                <Chart id="Chart1" data={this.state.chartData} className="mb20" timeSpan={StatisticsTimeSpan.Day} />

                <OverflowDiv>
                    <LeftSpan>Max</LeftSpan>
                    <RightSpan>23</RightSpan>
                </OverflowDiv>

                <OverflowDiv>
                    <LeftSpan>Min</LeftSpan>
                    <RightSpan>23</RightSpan>
                </OverflowDiv>

                <OverflowDiv>
                    <LeftSpan>Medelvärde</LeftSpan>
                    <RightSpan>23</RightSpan>
                </OverflowDiv>

            </div>

        </div>;
    }
}


export default StatisticsPage;