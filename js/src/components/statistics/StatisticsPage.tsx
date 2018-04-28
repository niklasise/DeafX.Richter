import * as React from 'react';
import { match, withRouter, RouteComponentProps } from 'react-router-dom';
import { Device } from 'models/device'
import Chart, { ChartComponentData } from './StatisticsChart';
import StatisticsTimeSpan from "models/statistics/statisticsTimeSpan"
//import StatisticApi from "../../Api/MockStatisticsApi";
//import DeviceApi from "../../Api/MockDeviceApi";
import DeviceApi from "api/deviceApi";
import StatisticApi from "api/statisticsApi";
import RadioButtonGroup from "components/shared/input/RadioButtonGroup";
import styled from "styled-components";

interface StatisticsPageParams {
    id: string
}

// interface StatisticsPageProps {
//     match?: match<StatisticsPageParams>
// }

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

class StatisticsPage extends React.Component<StatisticsPageProps, StatisticsPageState> {

    private devicePromise: Promise<Device>;
    private device: Device;
    private deviceId: string;

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
            StatisticApi.getStatistics(
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
                return 60 * 60 * 12; // 12 hours
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

        this.devicePromise = DeviceApi.getDevice(this.deviceId);

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
        return <div className="pageContainer">

            <div className="sectionContainer mb20">

                {this.state.error && <ErrorDiv>Ett fel har uppstått</ErrorDiv>}

                {!this.state.error &&
                    <div>
                        <h1> { this.state.device.title }</h1>

                        {this.state.loading && <LoaderImg src="/dist/img/loader.svg" />} 

                        {!this.state.loading && <Chart id="Chart1" data={this.state.chartData} className="mb20" timeSpan={this.state.selectedTimeSpan} startTime={this.state.chartStartTime} endTime={this.state.chartEndTime} />} 

                        <RadioButtonGroup options={TimespanOptions} selectedValue={this.state.selectedTimeSpan} onValueChanged={this.onRadioValueChanged} disabled={this.state.loading} />
                    </div>
                }
            </div>

        </div>;
    }
}

export default withRouter(StatisticsPage);