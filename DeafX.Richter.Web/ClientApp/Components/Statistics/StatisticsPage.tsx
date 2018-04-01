import * as React from 'react';
import { match } from 'react-router-dom';
import { Device } from '../../Models/Device'
import Chart, { ChartComponentData } from './StatisticsChart';
import StatisticsTimeSpan from "../../Models/Statistics/StatisticsTimeSpan"
import StatisticApi from "../../Api/MockStatisticsApi";
//import StatisticsBadge from "./StatisticsBadge"
import RadioButtonGroup from "../Shared/Input/RadioButtonGroup";
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
    selectedTimeSpan: StatisticsTimeSpan,
    loading: boolean
}

//const StatisticsBadgeMargin = styled(StatisticsBadge) `
//    margin-bottom: 20px;
//`

const TimespanOptions = [
    {
        name: "Dag", value: StatisticsTimeSpan.Day
    },
    {
        name: "Vecka", value: StatisticsTimeSpan.Week
    },
    {
        name: "Månad", value: StatisticsTimeSpan.Month
    },
    {
        name: "År", value: StatisticsTimeSpan.Year
    }
]

const LoaderImg = styled.img`
    display: block;
    margin: 150px auto;
    height: 35px;
    width: 35px;
`

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
                dataSets: []
            },
            loading: false
        }

        this.onRadioValueChanged = this.onRadioValueChanged.bind(this);
        this.loadData = this.loadData.bind(this);
    }

    private loadData(timeSpan: StatisticsTimeSpan) {

        this.setState({
            ...this.state,
            loading: true
        });

        var currentTimestamp = Math.floor(Date.now() / 1000);

        StatisticApi.getStatistics(
            "abc123",
            this.getFromTime(currentTimestamp, timeSpan),
            currentTimestamp,
            this.getMinumimInterval(timeSpan)).then((data) => {

                let chartData: ChartComponentData = {
                    dataSets: [
                        {
                            label: "Harp Darp",
                            data: data.map((d) => { return { x: d.timeStamp, y: d.data } })
                        }
                    ]
                }; 

                this.setState({
                    ...this.state,
                    loading: false,
                    chartData: chartData,
                    selectedTimeSpan: timeSpan
                }

            )
            })



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

    private onRadioValueChanged(value: any) {
        //this.setState({
        //    ...this.state,
        //    selectedTimeSpan: value as StatisticsTimeSpan
        //})

        //this.setState((prevState, props) => {
        //    return {
        //        ...prevState,
        //        selectedTimeSpan: value as StatisticsTimeSpan
        //    }
        //});

        this.loadData(value as StatisticsTimeSpan);
    }

    public componentDidMount() {
        this.loadData(StatisticsTimeSpan.Day);
    }

    public render() {
        return <div className="pageContainer">

            <div className="sectionContainer mb20">

                <h1>{this.state.device.title}</h1>

                {this.state.loading && <LoaderImg src="/dist/img/loader.svg" />} 

                {!this.state.loading && <Chart id="Chart1" data={this.state.chartData} className="mb20" timeSpan={this.state.selectedTimeSpan} />} 

                <RadioButtonGroup options={TimespanOptions} selectedValue={this.state.selectedTimeSpan} onValueChanged={this.onRadioValueChanged} loading={this.state.loading} />

                {/*
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
                */}
            </div>

        </div>;
    }
}


export default StatisticsPage;