﻿import * as React from 'react';
import { match, withRouter, RouteComponentProps } from 'react-router-dom';
import { Device } from 'models/device'
import Chart, { ChartComponentData } from './StatisticsChart';
import StatisticsTimeSpan from "models/statistics/statisticsTimeSpan"
import StatisticApi from "@api/statisticsApi";
import DeviceApi from "@api/deviceApi";
import RadioButtonGroup from "components/shared/input/RadioButtonGroup";
import styled from "styled-components";
import styles from "constants/styles";

interface StatisticsPageParams {
    id: string
}

interface StatisticsPageState {
    device: Device,
    chartData: ChartComponentData,
    chartStartTime: number,
    chartEndTime: number,
    selectedTimeSpan: StatisticsTimeSpan,
    loading: boolean,
    error: boolean
}

type StatisticsPageProps =
    StatisticsPageParams &
    RouteComponentProps<any>;

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

class StatisticsPage extends React.Component<StatisticsPageProps, StatisticsPageState> {

    private devicePromise: Promise<Device>;
    private device: Device;
    private deviceId: string;
    private stasticsApi : StatisticApi;
    private deviceApi : DeviceApi;

    constructor(props: StatisticsPageProps)
    {
        super(props);

        this.state = {
            error: false,
            selectedTimeSpan: StatisticsTimeSpan.Day,
            device: { id: "", title: "", deviceType: "", value: "", valueType: "", isUpdating: false, lastChanged: 0 },
            chartData: {
                dataSets: []
            },
            loading: false,
            chartStartTime: 0,
            chartEndTime: 0
        }

        this.deviceId = props.match.params.id;

        this.deviceApi = new DeviceApi();
        this.stasticsApi = new StatisticApi();

        this.onRadioValueChanged = this.onRadioValueChanged.bind(this);
        this.loadData = this.loadData.bind(this);
    }

    private loadData(timeSpan: StatisticsTimeSpan) {

        this.setState({
            ...this.state,
            selectedTimeSpan: timeSpan,
            loading: true
        });

        var currentTimestamp = Math.floor(Date.now() / 1000);
        var fromTimeStamp = this.getFromTime(currentTimestamp, timeSpan);

        Promise.all([
            this.devicePromise,
            this.stasticsApi.getStatistics(
                this.deviceId,
                fromTimeStamp - 60 * 60, // Get one hour more of data to "fill out" chart
                currentTimestamp,
                this.getMinumimInterval(timeSpan))
        ]).then((data) => {

            let chartData: ChartComponentData = {
                dataSets: [
                    {
                        label: data[0].title,
                        data: data[1].map((d) => { return { x: d.timeStamp, y: d.data } })
                    }
                ]
            };

            this.setState({
                ...this.state,
                loading: false,
                chartData: chartData,
                chartStartTime: fromTimeStamp,
                chartEndTime: currentTimestamp,
            });
        }).catch(() => {
            this.setState({
                ...this.state,
                error: true
            })
        });
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
                return 60 * 30; // 15 minutes
            case StatisticsTimeSpan.Week:
                return 60 * 60 * 6; // 6 hours
            case StatisticsTimeSpan.Month:
                return 60 * 60 * 24; // 24 hours
            case StatisticsTimeSpan.Year:
                return 60 * 60 * 24 * 7; // 7 days
            default:
                return 60 * 60 * 24; // 24 hours
        }
    }

    private onRadioValueChanged(value: any) {
        this.loadData(value as StatisticsTimeSpan);
    }

    private loadDeviceName() {

        this.devicePromise = this.deviceApi.getDevice(this.deviceId);

        this.devicePromise.then((data) => {
            this.setState({
                ...this.state,
                device: data
            })
        }).catch(() => {
            this.setState({
                ...this.state,
                error: true
            })
        })

    }

    public componentDidMount() {
        this.loadDeviceName();
        this.loadData(StatisticsTimeSpan.Day);
    }

    public render() {
        return ( 
            <ContainerDiv>
                <SectionDiv>

                    {this.state.error && <ErrorDiv>Ett fel har uppstått</ErrorDiv>}

                    {!this.state.error &&
                        <div>
                            <StyledH1> { this.state.device.title }</StyledH1>

                            {this.state.loading && <LoaderImg src="/dist/img/loader.svg" />} 

                            {!this.state.loading && <StyledChart id="Chart1" data={this.state.chartData} timeSpan={this.state.selectedTimeSpan} startTime={this.state.chartStartTime} endTime={this.state.chartEndTime} />} 

                            <RadioButtonGroup options={TimespanOptions} selectedValue={this.state.selectedTimeSpan} onValueChanged={this.onRadioValueChanged} disabled={this.state.loading} />
                        </div>
                    }
                </SectionDiv>
            </ContainerDiv>
        );
    }
}

const StyledChart = styled(Chart)`
    margin-bottom: 20px;
`

const StyledH1 = styled.h1`
    font-size: 30px;
    margin: 0;
`

const ContainerDiv = styled.div`
    max-width: 900px;
    padding: 20px 20px 0 20px;
    overflow: hidden;
    margin: 0 auto;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        padding: 15px 15px 0 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        padding: 10px 10px 0 10px;
    }
`

const SectionDiv = styled.div`
    background-color: #19a2de;
    color: #FFFFFF;
    border-radius: 5px;
    opacity: 0.7;
    padding: 20px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        padding: 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        padding: 10px;
    }
`

const LoaderImg = styled.img`
    display: block;
    margin: 150px auto;
    height: 35px;
    width: 35px;
`
const ErrorDiv = styled.div`
    text-align: center;
    margin-top: 150px;
    margin-bottom: 150px;
    font-size: 20px;
    font-weight: bold;    
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

export default withRouter(StatisticsPage);