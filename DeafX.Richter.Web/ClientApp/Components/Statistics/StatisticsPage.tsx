import * as React from 'react';
import { match } from 'react-router-dom';
import { Device } from '../../Models/Device'
import Chart, { ChartComponentData } from './Chart';
import StatisticsBadge from "./StatisticsBadge"
import styled from "styled-components";

interface StatisticsPageParams {
    id: string
}

interface StatisticsPageProps {
    match?: match<StatisticsPageParams>
}

interface StatisticsPageState {
    device: Device,
    chartData: ChartComponentData
}

const StatisticsBadgeMargin = styled(StatisticsBadge) `
    margin-bottom: 20px;
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
                //labels: ["2017-01-01", "2017-01-02", "2017-01-03", "2017-01-04", "2017-01-05", "2017-01-06"],
                dataSets: [
                //{
                //    label: "gästrum - fönster",
                //    data: [
                //        21,
                //        21.5,
                //        23,
                //        19,
                //        null,
                //        24,
                //    ]
                //},
                //{
                //    label: "kök - fönster",
                //    data: [
                //        11,
                //        20.5,
                //        12,
                //        13,
                //        24,
                //        25,
                //    ]
                //},
                //{
                //    label: "kök - fönster",
                //        data: [
                //            12,
                //            21.5,
                //            14,
                //            16,
                //            27,
                //            28,
                //        ]
                //},
                //{
                //    label: "kök - fönster",
                //        data: [
                //            14,
                //            22.5,
                //            11,
                //            17,
                //            22,
                //            24,
                //        ]
                //},
                //{
                //    label: "kök - fönster",
                //        data: [
                //            28,
                //            20.5,
                //            20,
                //            14,
                //            22,
                //            27,
                //        ]
                //},
                //{
                //    label: "kök - fönster",
                //        data: [
                //            12,
                //            21.5,
                //            19,
                //            17,
                //            27,
                //            24,
                //        ]
                //}
                {
                    label: "kök - fönster",
                    data: [
                        { x: 1, y: 12 },
                        { x: 2, y: 21.5 },
                        { x: 3, y: 19 },
                        { x: 4, y: 17 },
                        { x: 5, y: 27 },
                        { x: 6, y: 24 }
                    ]
                },
                {
                    label: "kök - dörr",
                    data: [
                        { x: 0.5, y: 15 },
                        { x: 1.5, y: 18 },
                        { x: 4.5, y: 21 },
                        { x: 5.5, y: 24 }
                    ]
                }]
            }
        }
    }

    public render() {
        return <div className="pageContainer">

            <div className="sectionContainer mb20">

                <h1>{this.state.device.title}</h1>

                <Chart id="Chart1" data={this.state.chartData} className="mb20" />

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

            {/*
            <StatisticsBadgeMargin label="Max" value="23" />

            <StatisticsBadgeMargin label="Min" value="23" />

            <StatisticsBadgeMargin label="Medelvärde" value="23" />
            */}
        </div>;
    }
}


export default StatisticsPage;