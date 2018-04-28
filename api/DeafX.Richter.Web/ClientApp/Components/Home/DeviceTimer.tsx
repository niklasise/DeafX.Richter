import * as React from "react";
import { Device as DeviceModel, ToggleDevice } from "../../Models/Device";

export interface DeviceTimerProps {
    device: ToggleDevice;
    onTimerClick(source: DeviceModel): void;
    onTimerAbortClick(source: DeviceModel): void;
}

interface DeviceTimerState {
    timeLeft: number;
}

export default class DeviceTimer extends React.Component<DeviceTimerProps, DeviceTimerState> {

    private timeoutId: number;
    private timeMounted: number;

    constructor(props: DeviceTimerProps) {
        super();

        this.tick = this.tick.bind(this);
        this.onTimerAbortClick = this.onTimerAbortClick.bind(this);
        this.onTimerClick = this.onTimerClick.bind(this);
        this.getIconColor = this.getIconColor.bind(this);

        this.state = {
            timeLeft: props.device.timer == null ? 0 : props.device.timer.timerValue
        };
    }

    componentDidMount() {
        this.timeMounted = this.getTime();
        this.timeoutId = setTimeout(this.tick, 1000);
    }

    componentWillUnmount() {
        clearTimeout(this.timeoutId);
    }

    componentWillReceiveProps(nextProps: DeviceTimerProps) {
        if (nextProps.device.timer === this.props.device.timer)
        {
            return;
        }

        this.timeMounted = this.getTime();

        var timerValue = nextProps.device.timer == null ? 0 : nextProps.device.timer.timerValue;

        this.setState({
            ...this.state,
            timeLeft: this.getTimeLeft(timerValue)
        });

        if (this.timeoutId === 0) {
            this.timeoutId = setTimeout(this.tick, 1000);
        }
    }

    tick() {
        var timerValue = this.props.device.timer == null ? 0 : this.props.device.timer.timerValue;

        this.setState({
            ...this.state,
            timeLeft: this.getTimeLeft(timerValue)
        });

        if (this.state.timeLeft > 0)
        {
            this.timeoutId = setTimeout(this.tick, 1000);
        }
        else
        {
            this.timeoutId = 0;    
        }
    }

    getTimeLeft(timerValue: number): number {
        let timeLeft = this.timeMounted + timerValue - this.getTime();

        return timeLeft > 0 ? timeLeft : 0;
    }

    getTime() : number {
        return Math.floor(Date.now() / 1000);
    }

    getIconColor(): string {
        return this.props.device.timer == null ?
            "" :
            this.props.device.timer.stateToSet ? "green" : "red";
    }

    formatTimerValue(seconds: number): string {
        let h: any = Math.floor(seconds / 3600);
        let m: any = Math.floor(seconds % 3600 / 60);
        let s: any = Math.floor(seconds % 3600 % 60);

        h = h >= 10 ? h : '0' + h;
        m = m >= 10 ? m : '0' + m;
        s = s >= 10 ? s : '0' + s;

        return h + ':' + m + ':' + s;
    }

    onTimerClick(event: React.MouseEvent<HTMLDivElement>): void {
        event.stopPropagation();
        this.props.onTimerClick(this.props.device);
    }

    onTimerAbortClick(event: React.MouseEvent<HTMLDivElement>): void {
        event.stopPropagation();
        this.props.onTimerAbortClick(this.props.device);
    }

    public render() {
        return <div
            className="clickable"
            onClick={!!this.state.timeLeft ? this.onTimerAbortClick : this.onTimerClick}>
            <i className={"fa fa-clock-o " + this.getIconColor()} />
                {!!this.state.timeLeft && <span>{this.formatTimerValue(this.state.timeLeft)}</span>}
        </div>;
    }

}


